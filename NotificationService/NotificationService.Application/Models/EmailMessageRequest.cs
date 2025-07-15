namespace CertMailer.NotificationService.Application.Models;

public record EmailMessageRequest(
    string RecipientEmail, string RecipientName, 
    string Subject, string MailBody,
    IEnumerable<EmailAttachment>? Attachments = null);