namespace CertificateGen.WebUI.Models;

public class AddTemplateRequest
{
    public required string Name { get; set; }
    public required string Title { get; set; }
    public required string Subtitle { get; set; }
    public required string Description { get; set; }
    public IFormFile? BackgroundFile { get; set; }
}