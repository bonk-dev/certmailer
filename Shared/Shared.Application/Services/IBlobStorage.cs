namespace CertMailer.Shared.Application.Services;

public interface IBlobStorage
{
    Task DownloadAsync(string blobUri, Stream stream, CancellationToken cancellationToken);
    Task DownloadAsync(string container, string blobName, Stream stream, CancellationToken cancellationToken);
    Task<Uri> UploadAsync(string container, string blobName, Stream stream, CancellationToken cancellationToken);
}