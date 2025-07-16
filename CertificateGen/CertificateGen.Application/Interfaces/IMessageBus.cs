using CertMailer.CertificateGen.Application.Dto;
using CertMailer.Shared.Application.Dto;

namespace CertMailer.CertificateGen.Application.Interfaces;

public interface IMessageBus
{
    Task PublishCertificateGeneratedAsync(CertificateGeneratedDto dto);
}