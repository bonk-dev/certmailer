namespace CertMailer.NotificationService.Application.Commands;

public class SendCertificateEmailCommand
{
    public required string Email { get; set; }
    public required Guid CertificateId { get; set; }
    public required string CertificateUri { get; set; }
}