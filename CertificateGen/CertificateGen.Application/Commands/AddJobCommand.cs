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

    public AddJobCommandHandler(IJobStorage storage, IMediator mediator)
    {
        _storage = storage;
        _mediator = mediator;
    }
    
    public async Task<Guid> Handle(AddJobCommand request, CancellationToken cancellationToken)
    {
        var job = await _storage.AddJobAsync(
            request.ExcelParsedEvent.BatchId, 
            request.ExcelParsedEvent.Participants);
        // Fire and forget task, I would use Hangfire for this, TODO: maybe later
        _ = _mediator.Publish(new JobAddedNotification
        {
            BatchId = job.BatchId
        }, cancellationToken);
        return job.BatchId;
    }
}

public class JobAddedNotification : INotification
{
    public required Guid BatchId { get; init; }
}