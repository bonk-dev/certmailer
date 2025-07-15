using CertMailer.ExcelParser.Application.Commands;
using CertMailer.ExcelParser.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CertMailer.ExcelParser.Application.NotificationHandlers;

public class ParseOnJobAdded : INotificationHandler<JobAddedNotification>
{
    private readonly ILogger<ParseOnJobAdded> _logger;
    private readonly IExcelService _excelService;
    private readonly IJobStorage _storage;

    public ParseOnJobAdded(
        ILogger<ParseOnJobAdded> logger, 
        IExcelService excelService,
        IJobStorage storage)
    {
        _logger = logger;
        _excelService = excelService;
        _storage = storage;
    }
    
    public async Task Handle(JobAddedNotification notification, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Received job notification: {0}", notification.BatchId);
        var job = await _storage.GetJobAsync(notification.BatchId);

        var result = _excelService.Parse(job.Data!.Memory);
        job.Result = result;
        
        _logger.LogDebug("Job done: {0}, {1}", job.BatchId, job.Result.Success);
        
        // TODO: Send to message bus
    }
}