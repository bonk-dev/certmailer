namespace CertMailer.Application.Models.Settings;

public class MailSettings
{
    public required string FromAddress { get; set; }
    public required string FromDisplayName { get; set; }
    
    public required string Host { get; set; }
    public required int Port { get; set; }
    
    public required string AuthUsername { get; set; }
    public required string AuthPassword { get; set; }
}