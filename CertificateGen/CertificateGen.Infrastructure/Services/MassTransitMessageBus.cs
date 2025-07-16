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
    
    public async Task PublishCertificateGeneratedAsync(
        Guid batchId, ParticipantDto participant, Guid certificateId, string certificateUri)
    {
        await _endpoint.Publish(new CertificateGenerated
        {
            Participant = participant,
            BatchId = batchId,
            Certificate = new CertificateInfoDto
            {
                CertificateId = certificateId,
                CertificateUri = certificateUri
            }
        });
    }
}