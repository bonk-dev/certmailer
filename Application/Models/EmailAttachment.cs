namespace CertMailer.Application.Models;

public readonly struct EmailAttachment
{
    public required string FileName { get; init; }
    public required string ContentType { get; init; }
    public required byte[] Data { get; init; }
}