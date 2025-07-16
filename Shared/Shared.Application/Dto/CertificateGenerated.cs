namespace CertMailer.Shared.Application.Dto;

public class CertificateGenerated
{
    public required Guid BatchId { get; set; }
    public required ParticipantDto Participant { get; set; }
    public required CertificateInfoDto Certificate { get; set; }
}