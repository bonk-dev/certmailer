using CertMailer.Shared.Application.Dto;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace CertMailer.NotificationService.Infrastructure.Consumers;

public class CertificateGeneratedConsumer : IConsumer<CertificateGenerated>
{
    private readonly ILogger<CertificateGeneratedConsumer> _logger;

    public CertificateGeneratedConsumer(ILogger<CertificateGeneratedConsumer> logger)
    {
        _logger = logger;
    }
    
    public Task Consume(ConsumeContext<CertificateGenerated> context)
    {
        _logger.LogDebug("Consuming CertificateGenerated: (batch: {0}) {1}", 
            context.Message.BatchId, context.Message.Certificate.CertificateId);
        return Task.CompletedTask;
    }
}