namespace CertMailer.Shared.Application.Dto;

public class ParticipantCertDto : ParticipantDto
{
    public required Guid CertificateId { get; set; }
    public required string CertificateUri { get; set; }
}