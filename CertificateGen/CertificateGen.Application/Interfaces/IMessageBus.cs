namespace CertMailer.CertificateGen.Application.Interfaces;

public interface IMessageBus
{
    Task PublishCertificateGeneratedAsync(Guid batchId, string email, Guid certificateId, string certificateUri);
}