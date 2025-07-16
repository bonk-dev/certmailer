using CertMailer.CertificateGen.Application.Commands;
using CertMailer.CertificateGen.Application.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CertMailer.CertificateGen.Application.NotificationHandlers;

public class GenerateOnJobAdded : INotificationHandler<JobAddedNotification>
{
    private readonly ILogger<GenerateOnJobAdded> _logger;
    private readonly IMediator _mediator;

    public GenerateOnJobAdded(
        ILogger<GenerateOnJobAdded> logger, 
        IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    public async Task Handle(JobAddedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling JobAddedNotification: {0}", notification.BatchId);
        var job = await _mediator.Send(new GetJobCommand
        {
            BatchId = notification.BatchId
        }, cancellationToken);
        if (job == null)
        {
            _logger.LogError("Job {0} doesn't exist", notification.BatchId);
            return;
        }

        job.JobStatus = Job.Status.InProgress;

        if (!job.Results.Any())
        {
            job.Results.AddRange(job.ParticipantsDto.Select(p => new JobCertificateResult
            {
                Participant = p,
                CertificateId = default,
                CertificateUri = string.Empty
            }));
        }

        try
        {
            foreach (var p in job.Results
                         .Where(r => string.IsNullOrEmpty(r.CertificateUri)))
            {
                var certResult = await _mediator.Send(new GenerateCertificateCommand
                {
                    BatchId = notification.BatchId,
                    Participant = p.Participant
                }, cancellationToken);

                var result = new JobCertificateResult
                {
                    Participant = p.Participant,
                    CertificateId = certResult.CertificateId,
                    CertificateUri = certResult.CertificateUri
                };
                p.CertificateUri = result.CertificateUri;
                p.CertificateId = result.CertificateId;

                _logger.LogDebug("Certificate generated (batch: {0}): {1}, uri: {2}",
                    job.BatchId, p.Participant, result.CertificateUri);

                await _mediator.Send(new SendCertificateEventCommand
                {
                    BatchId = notification.BatchId,
                    JobResult = result,
                    MailTemplateId = job.MailTemplateId,
                    SubjectTemplateId = job.SubjectTemplateId
                }, cancellationToken);
            }
        }
        catch
        {
            job.JobStatus = Job.Status.Failed;
            throw;
        }

        job.JobStatus = Job.Status.Done;

        // TODO: UpdateJobCommand
    }
}