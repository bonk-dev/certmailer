namespace CertMailer.CertificateGen.Domain.Entities;

public class CertificateTemplate
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Title { get; set; }
    public required string Subtitle { get; set; }
    public required string Description { get; set; }
    public string? BackgroundUri { get; set; }
}