namespace CertMailer.Shared.Application.Dto;

public class CertificateInfoDto
{
    public required Guid CertificateId { get; set; }
    public required string CertificateUri { get; set; }
}