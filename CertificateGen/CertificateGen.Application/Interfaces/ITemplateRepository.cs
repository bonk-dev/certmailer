using CertMailer.CertificateGen.Domain.Entities;

namespace CertMailer.CertificateGen.Application.Interfaces;

public interface ITemplateRepository
{
    Task<IEnumerable<CertificateTemplate>> GetAllTemplatesAsync();
    Task<CertificateTemplate> GetDefaultTemplateAsync();
    Task<CertificateTemplate?> GetTemplateAsync(int id);
    Task<CertificateTemplate> AddTemplateAsync(CertificateTemplate template);
}