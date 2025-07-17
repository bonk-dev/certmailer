using CertMailer.CertificateGen.Application.Interfaces;
using CertMailer.CertificateGen.Domain.Entities;

namespace CertMailer.CertificateGen.Infrastructure.Services;

public class InMemoryTemplateRepository : ITemplateRepository
{
    private const int DefaultTemplateId = 1;
    private readonly List<CertificateTemplate> _templates =
    [
        new CertificateTemplate
        {
            Name = "Default",
            BackgroundUri = null,
            Description = "za ukończenie kursu „{0}” w dniu {1}",
            Subtitle = "Ten certyfikat został przyznany dla",
            Title = "Certyfikat ukończenia kursu",
            Id = DefaultTemplateId
        }
    ];

    public Task<IEnumerable<CertificateTemplate>> GetAllTemplatesAsync() => 
        Task.FromResult(_templates.AsEnumerable());

    public async Task<CertificateTemplate> GetDefaultTemplateAsync() => 
        (await GetTemplateAsync(DefaultTemplateId))!;

    public Task<CertificateTemplate?> GetTemplateAsync(int id) => 
        Task.FromResult(_templates.SingleOrDefault(t => t.Id == id));

    public async Task<CertificateTemplate> AddTemplateAsync(CertificateTemplate template)
    {
        var existing = await GetTemplateAsync(template.Id);
        if (existing != null)
        {
            return existing;
        }

        template.Id = _templates.Max(t => t.Id) + 1;
        _templates.Add(template); 

        return template;
    }

    public async Task<CertificateTemplate> UpdateTemplateAsync(CertificateTemplate template)
    {
        var existingTemplate = await GetTemplateAsync(template.Id);
        if (existingTemplate == null)
        {
            throw new Exception("Template not found");
        }

        existingTemplate.Description = template.Description;
        existingTemplate.Name = template.Name;
        existingTemplate.Subtitle = template.Subtitle;
        existingTemplate.Title = template.Title;
        existingTemplate.BackgroundUri = template.BackgroundUri;

        return existingTemplate;
    }
}