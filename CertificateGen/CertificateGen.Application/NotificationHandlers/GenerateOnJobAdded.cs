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

        try
        {
            foreach (var p in job.ParticipantsDto)
            {
                var certResult = await _mediator.Send(new GenerateCertificateCommand
                {
                    BatchId = notification.BatchId,
                    Participant = p
                }, cancellationToken);

                var result = new JobCertificateResult
                {
                    Participant = p,
                    CertificateId = certResult.CertificateId,
                    CertificateUri = certResult.CertificateUri
                };
                job.Results.Add(result);

                _logger.LogDebug("Certificate generated (batch: {1}): {0}, uri: {2}",
                    p.Email, job.BatchId, result.CertificateUri);
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