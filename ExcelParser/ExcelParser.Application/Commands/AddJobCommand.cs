using CertMailer.ExcelParser.Application.Interfaces;
using MediatR;

namespace CertMailer.ExcelParser.Application.Commands;

public class AddJobCommand : IRequest<Guid>
{
    public required Stream FileStream { get; set; }
}

public class AddJobCommandHandler : IRequestHandler<AddJobCommand, Guid>
{
    private readonly IJobStorage _jobStorage;
    private readonly IMediator _mediator;

    public AddJobCommandHandler(IJobStorage jobStorage, IMediator mediator)
    {
        _jobStorage = jobStorage;
        _mediator = mediator;
    }
    
    public async Task<Guid> Handle(AddJobCommand request, CancellationToken cancellationToken)
    {
        var guid = await _jobStorage.StoreFileAsync(request.FileStream);
        await _mediator.Publish(new JobAddedNotification
        {
            BatchId = guid.BatchId
        }, cancellationToken);
        return guid.BatchId;
    }
}

public class JobAddedNotification : INotification
{
    public required Guid BatchId { get; init; }
}