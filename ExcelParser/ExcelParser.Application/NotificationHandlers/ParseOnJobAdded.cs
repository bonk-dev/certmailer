using CertMailer.ExcelParser.Application.Commands;
using CertMailer.ExcelParser.Application.Dto;
using CertMailer.ExcelParser.Application.Interfaces;
using CertMailer.Shared.Application.Dto;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CertMailer.ExcelParser.Application.NotificationHandlers;

public class ParseOnJobAdded : INotificationHandler<JobAddedNotification>
{
    private readonly ILogger<ParseOnJobAdded> _logger;
    private readonly IExcelService _excelService;
    private readonly IJobStorage _storage;
    private readonly IMessageBus _messageBus;

    public ParseOnJobAdded(
        ILogger<ParseOnJobAdded> logger, 
        IExcelService excelService,
        IJobStorage storage,
        IMessageBus messageBus)
    {
        _logger = logger;
        _excelService = excelService;
        _storage = storage;
        _messageBus = messageBus;
    }
    
    public async Task Handle(JobAddedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Received job notification: {0}", notification.BatchId);
        var job = await _storage.GetJobAsync(notification.BatchId);

        var result = _excelService.Parse(job.Data!.Memory);
        job.Result = result;
        
        _logger.LogDebug("Job done: {0}, {1}", job.BatchId, job.Result.Success);
        
        if (result is { Success: true, Data: not null })
        {
            _logger.LogDebug("Sending ExcelParsed event on message bus: {0}", job.BatchId);

            var participantDtos = result.Data.Select(p => new ParticipantDto
            {
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                CourseName = p.CourseName,
                CompletionDate = p.CompletionDate
            });
            var eventDto = new ExcelParsedDto
            {
                BatchId = job.BatchId,
                Participants = participantDtos.ToArray(),
                MailTemplateId = job.MailTemplateId,
                SubjectTemplateId = job.SubjectTemplateId
            };
            await _messageBus.PublishExcelParsedAsync(eventDto);
        }
    }
}