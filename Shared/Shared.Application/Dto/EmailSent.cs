namespace CertMailer.Shared.Application.Dto;

public class EmailSent
{
    public required Guid BatchId { get; set; }
    public required Guid CertificateId { get; set; }
}