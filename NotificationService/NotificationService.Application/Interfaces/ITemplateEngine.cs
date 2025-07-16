using NotificationService.Domain.Entities;

namespace CertMailer.NotificationService.Application.Interfaces;

public interface ITemplateEngine
{
    string ApplyTemplate(MailTemplate template, IReadOnlyDictionary<string, string> values);
}