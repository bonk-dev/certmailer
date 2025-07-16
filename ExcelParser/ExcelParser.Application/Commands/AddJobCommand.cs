using CertMailer.ExcelParser.Application.Dto;
using CertMailer.ExcelParser.Application.Interfaces;
using MediatR;

namespace CertMailer.ExcelParser.Application.Commands;

public class AddJobCommand : IRequest<Guid>
{
    public required Stream FileStream { get; set; }
    public int? MailTemplateId { get; set; }
    public int? SubjectTemplateId { get; set; }
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
        var guid = await _jobStorage.AddJobAsync(new JobCreationDto
        {
            Stream = request.FileStream,
            MailTemplateId = request.MailTemplateId,
            SubjectTemplateId = request.SubjectTemplateId
        });
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