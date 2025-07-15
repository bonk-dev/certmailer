namespace CertMailer.CertificateGen.Application.Models;

public readonly struct CertificateOptions
{
    /// <summary>
    /// Stream to a PDF file which is going to be underlain under the certificate contents.
    /// </summary>
    public Stream? BackgroundDocument { get; init; }
    
    public required string Title { get; init; }
    public required string Subtitle { get; init; }
    public required string DescriptionFormat { get; init; }

    public static CertificateOptions Default { get; } = new()
    {
        BackgroundDocument = null,
        Title = "Certyfikat ukończenia kursu",
        Subtitle = "Ten certyfikat został przyznany dla",
        DescriptionFormat = "za ukończenie kursu „{0}” w dniu {1}"
    };
}