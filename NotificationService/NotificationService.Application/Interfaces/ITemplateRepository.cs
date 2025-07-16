using NotificationService.Domain.Entities;

namespace CertMailer.NotificationService.Application.Interfaces;

public interface ITemplateRepository
{
    Task<MailTemplate> GetDefaultTemplateAsync();
    Task<MailTemplate> GetDefaultSubjectTemplateAsync();
    Task<MailTemplate?> GetTemplateAsync(int id);
}