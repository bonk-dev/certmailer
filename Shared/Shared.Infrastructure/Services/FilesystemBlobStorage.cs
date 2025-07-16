using CertMailer.Shared.Application.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Infrastructure.Models;

namespace Shared.Infrastructure.Services;

public class FilesystemBlobStorage : IBlobStorage
{
    private readonly ILogger<FilesystemBlobStorage> _logger;
    private readonly FilesystemBlobStorageOptions _settings;

    public FilesystemBlobStorage(IOptions<FilesystemBlobStorageOptions> options, ILogger<FilesystemBlobStorage> logger)
    {
        _logger = logger;
        _settings = options.Value;
    }

    public async Task DownloadAsync(string blobUri, Stream stream, CancellationToken cancellationToken)
    {
        var uri = new Uri(blobUri);
        if (uri.Scheme != "blob")
        {
            throw new Exception("Invalid scheme (must be blob://)");
        }

        if (uri.Segments.Length < 3)
        {
            throw new Exception("Invalid URI (not enough segments)");
        }

        var uriPath = uri.AbsolutePath.TrimStart('/');
        var path = Path.Combine(_settings.RootDirectory, uriPath);
        
        _logger.LogDebug("Downloading BLOB from {0} (actual path: {1})", blobUri, path);
        if (!File.Exists(path))
        {
            throw new Exception("Blob doesn't exist");
        }

        await using var fStream = File.OpenRead(path);
        await fStream.CopyToAsync(stream, cancellationToken);
    }

    public Task DownloadAsync(string container, string blobName, Stream stream, CancellationToken cancellationToken) => 
        throw new NotImplementedException();

    public async Task<Uri> UploadAsync(string container, string blobName, Stream stream, CancellationToken cancellationToken)
    {
        var result = GetUri(container, blobName);
        
        await using var fStream = new FileStream(result.FilesystemPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
        await stream.CopyToAsync(fStream, cancellationToken);

        _logger.LogInformation("New BLOB resource at {0}", result.BlobUri);
        _logger.LogDebug("BLOB filesystem path: {0}", result.FilesystemPath);
        
        return new Uri(result.BlobUri);
    }

    private GetUriResult GetUri(string container, string blobName)
    {
        blobName = NaiveSanitizeName(blobName);
        container = NaiveSanitizeName(container);
        var blobNameDir = Path.GetDirectoryName(blobName);
        
        var dir = Path.Combine(_settings.RootDirectory, container, blobNameDir ?? string.Empty);
        Directory.CreateDirectory(dir);

        var file = Path.GetFileName(blobName);
        var uri = string.IsNullOrEmpty(blobNameDir) 
            ? $"blob:///{container}/{file}"
            : $"blob:///{container}/{blobNameDir}/{file}";
        return new GetUriResult
        {
            BlobUri = uri,
            FilesystemPath = Path.Combine(dir, file)
        };
    }

    private readonly struct GetUriResult
    {
        public required string BlobUri { get; init; }
        public required string FilesystemPath { get; init; }
    }

    private static string NaiveSanitizeName(string s) =>
        s.Replace("../", string.Empty)
        .Replace("..", string.Empty);
}