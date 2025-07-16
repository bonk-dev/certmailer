using CertMailer.CertificateGen.Application.Dto;
using CertMailer.CertificateGen.Application.Interfaces;
using CertMailer.Shared.Application.Dto;
using MassTransit;

namespace CertMailer.CertificateGen.Infrastructure.Services;

public class MassTransitMessageBus : IMessageBus
{
    private readonly IPublishEndpoint _endpoint;

    public MassTransitMessageBus(IPublishEndpoint endpoint)
    {
        _endpoint = endpoint;
    }
    
    public async Task PublishCertificateGeneratedAsync(CertificateGeneratedDto dto)
    {
        await _endpoint.Publish(new CertificateGenerated
        {
            Participant = dto.Participant,
            BatchId = dto.BatchId,
            Certificate = dto.Certificate,
            MailTemplateId = dto.MailTemplateId,
            SubjectTemplateId = dto.SubjectTemplateId
        });
    }
}