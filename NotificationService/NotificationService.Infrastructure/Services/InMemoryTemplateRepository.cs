using CertMailer.NotificationService.Application.Interfaces;
using NotificationService.Domain.Entities;

namespace CertMailer.NotificationService.Infrastructure.Services;

public class InMemoryTemplateRepository : ITemplateRepository
{
    private const int DefaultTemplateId = 1;
    private const int DefaultSubjectTemplateId = 2;
    private readonly List<MailTemplate> _templates = [
        new MailTemplate
        {
            Id = DefaultTemplateId,
            Name = "Domyślny",
            Template = """
                       <p>Cześć {FirstName}!<br>
                       Gratulujemy ukończenia kursu {CourseName}.<br>
                       W załączniku znajdziesz swój certyfikat.</p>
                       
                       <p>Pozdrawiamy,<br>
                       dpago.dev</p>
                       """
        },
        new MailTemplate
        {
            Id = DefaultSubjectTemplateId,
            Name = "Domyślny (temat)",
            Template = "Certyfikat za {CourseName}"
        }
    ]; 
    
    public async Task<MailTemplate> GetDefaultTemplateAsync() => 
        (await GetTemplateAsync(DefaultTemplateId))!;

    public async Task<MailTemplate> GetDefaultSubjectTemplateAsync() => 
        (await GetTemplateAsync(DefaultSubjectTemplateId))!;

    public Task<MailTemplate?> GetTemplateAsync(int id) => 
        Task.FromResult(_templates.SingleOrDefault(t => t.Id == id));
}