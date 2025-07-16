using CertMailer.NotificationService.Application.Commands;
using CertMailer.Shared.Application.Dto;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CertMailer.NotificationService.Infrastructure.Consumers;

public class CertificateGeneratedConsumer : IConsumer<CertificateGenerated>
{
    private readonly ILogger<CertificateGeneratedConsumer> _logger;
    private readonly IMediator _mediator;

    public CertificateGeneratedConsumer(ILogger<CertificateGeneratedConsumer> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    public async Task Consume(ConsumeContext<CertificateGenerated> context)
    {
        _logger.LogDebug("Consuming CertificateGenerated: (batch: {0}) {1}", 
            context.Message.BatchId, context.Message.Certificate.CertificateId);
        await _mediator.Send(new SendCertificateEmailCommand
        {
            BatchId = context.Message.BatchId,
            Participant = context.Message.Participant,
            CertificateId = context.Message.Certificate.CertificateId,
            CertificateUri = context.Message.Certificate.CertificateUri,
            MailTemplateId = context.Message.MailTemplateId,
            SubjectTemplateId = context.Message.SubjectTemplateId
        });
    }
}