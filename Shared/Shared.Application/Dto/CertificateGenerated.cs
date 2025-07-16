namespace CertMailer.Shared.Application.Dto;

public class CertificateGenerated
{
    public required Guid BatchId { get; set; }
    public required string Email { get; set; }
    public required string To { get; set; }
    public required CertificateInfoDto Certificate { get; set; }
}