using CertMailer.NotificationService.Application.Interfaces;
using NotificationService.Domain.Entities;

namespace CertMailer.NotificationService.Infrastructure.Services;

public class NaiveTemplateEngine : ITemplateEngine
{
    public string ApplyTemplate(MailTemplate template, IReadOnlyDictionary<string, string> values)
    {
        var s = template.Template;
        foreach (var pair in values)
        {
            s = s.Replace(pair.Key, pair.Value);
        }

        return s;
    }
}