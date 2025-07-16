using CertMailer.ExcelParser.Application.Interfaces;
using CertMailer.Shared.Application.Dto;
using MassTransit;

namespace CertMailer.ExcelParser.Infrastructure.Consumers;

public class CertificateGeneratedConsumer : IConsumer<CertificateGenerated>
{
    private readonly IJobStorage _jobStorage;

    public CertificateGeneratedConsumer(IJobStorage jobStorage)
    {
        _jobStorage = jobStorage;
    }
    
    public async Task Consume(ConsumeContext<CertificateGenerated> context)
    {
        var job = await _jobStorage.GetJobAsync(context.Message.BatchId);
        if (job == null)
        {
            throw new Exception($"Batch {context.Message.BatchId} doesn't exist");
        }

        job.JobStatus.CertificatesGenerated++;
        await _jobStorage.UpdateJobAsync(job);
    }
}