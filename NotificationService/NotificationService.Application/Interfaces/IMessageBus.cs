using CertMailer.Shared.Application.Dto;

namespace CertMailer.NotificationService.Application.Interfaces;

public interface IMessageBus
{
    Task SendEmailSentEventAsync(EmailSent eventData);
}