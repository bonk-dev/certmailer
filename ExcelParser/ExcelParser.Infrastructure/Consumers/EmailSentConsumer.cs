using CertMailer.ExcelParser.Application.Interfaces;
using CertMailer.Shared.Application.Dto;
using MassTransit;

namespace CertMailer.ExcelParser.Infrastructure.Consumers;

public class EmailSentConsumer : IConsumer<EmailSent>
{
    private readonly IJobStorage _jobStorage;

    public EmailSentConsumer(IJobStorage jobStorage)
    {
        _jobStorage = jobStorage;
    }
    
    public async Task Consume(ConsumeContext<EmailSent> context)
    {
        var job = await _jobStorage.GetJobAsync(context.Message.BatchId);
        if (job == null)
        {
            throw new Exception($"Batch {context.Message.BatchId} doesn't exist");
        }

        job.JobStatus.MailsSent++;
        await _jobStorage.UpdateJobAsync(job);
    }
}