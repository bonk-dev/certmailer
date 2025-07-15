using CertMailer.NotificationService.Application.Models;

namespace CertMailer.NotificationService.Application.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessageRequest request);
}