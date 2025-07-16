using CertMailer.Shared.Application.Dto;

namespace CertMailer.CertificateGen.Application.Interfaces;

public interface IMessageBus
{
    Task PublishCertificateGeneratedAsync(
        Guid batchId, ParticipantDto participant, Guid certificateId, string certificateUri);
}