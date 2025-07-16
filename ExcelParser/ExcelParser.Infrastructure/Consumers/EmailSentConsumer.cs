using CertMailer.ExcelParser.Application.Interfaces;
using CertMailer.Shared.Application.Dto;
using ExcelParser.Domain.Entities;
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

        job.JobStatus.Increment(JobStatus.Prop.Emails);
        await _jobStorage.UpdateJobAsync(job);
    }
}