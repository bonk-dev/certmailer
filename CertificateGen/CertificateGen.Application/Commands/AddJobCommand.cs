using CertMailer.CertificateGen.Application.Dto;
using CertMailer.CertificateGen.Application.Interfaces;
using CertMailer.Shared.Application.Dto;
using MediatR;

namespace CertMailer.CertificateGen.Application.Commands;

public class AddJobCommand : IRequest<Guid>
{
    public required ExcelParsed ExcelParsedEvent { get; set; }
}

public class AddJobCommandHandler : IRequestHandler<AddJobCommand, Guid>
{
    private readonly IJobStorage _storage;
    private readonly IMediator _mediator;
    private readonly IBackgroundJobService _jobService;

    public AddJobCommandHandler(IJobStorage storage, IMediator mediator, IBackgroundJobService jobService)
    {
        _storage = storage;
        _mediator = mediator;
        _jobService = jobService;
    }
    
    public async Task<Guid> Handle(AddJobCommand request, CancellationToken cancellationToken)
    {
        var job = await _storage.AddJobAsync(new CreateJobDto
        {
            BatchId = request.ExcelParsedEvent.BatchId, 
            Participants = request.ExcelParsedEvent.Participants,
            MailTemplateId = request.ExcelParsedEvent.MailTemplateId,
            SubjectTemplateId = request.ExcelParsedEvent.SubjectTemplateId
        });
        _ = _jobService.Enqueue(() => _mediator.Publish(new JobAddedNotification()
        {
            BatchId = job.BatchId
        }, cancellationToken));
        return job.BatchId;
    }
}

public class JobAddedNotification : INotification
{
    public required Guid BatchId { get; init; }
}