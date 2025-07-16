using NotificationService.Domain.Entities;

namespace CertMailer.NotificationService.Application.Interfaces;

public interface ITemplateRepository
{
    Task<MailTemplate> GetDefaultTemplateAsync();
    Task<MailTemplate?> GetTemplateAsync(int id);
}