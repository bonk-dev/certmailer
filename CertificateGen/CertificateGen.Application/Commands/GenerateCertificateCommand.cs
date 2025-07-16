using CertMailer.CertificateGen.Application.Interfaces;
using CertMailer.Shared.Application.Dto;
using CertMailer.Shared.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CertMailer.CertificateGen.Application.Commands;

public class GenerateCertificateCommand : IRequest<GenerateCertificateResult>
{
    public required Guid BatchId { get; set; }
    public required ParticipantDto Participant { get; set; }
}

public class GenerateCertificateResult
{
    public required Guid CertificateId { get; set; }
    public required string CertificateUri { get; set; }
}

public class GenerateCertificateCommandHandler : IRequestHandler<GenerateCertificateCommand, GenerateCertificateResult>
{
    private readonly ILogger<GenerateCertificateCommandHandler> _logger;
    private readonly ICertificateService _certificateService;

    public GenerateCertificateCommandHandler(
        ILogger<GenerateCertificateCommandHandler> logger,
        ICertificateService certificateService)
    {
        _logger = logger;
        _certificateService = certificateService;
    }
    
    public async Task<GenerateCertificateResult> Handle(GenerateCertificateCommand request, CancellationToken cancellationToken)
    {
        var certId = Guid.NewGuid();
        _logger.LogDebug("Generating cert (batch: {0}): {1}", request.BatchId, certId);

        var baseDir = Path.Combine(Path.GetTempPath(), request.BatchId.ToString());
        _logger.LogDebug("Ensuring dir exists: {0}", baseDir);
        Directory.CreateDirectory(baseDir);
        
        var certPath = Path.Combine(baseDir, certId + ".pdf");
        _logger.LogDebug("Certificate file: {0}", certPath);
        await using (var stream = new FileStream(certPath, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read))
        {
            using var memStream = new MemoryStream();
            _certificateService.GeneratePdf(
                new Participant(
                    request.Participant.FirstName,
                    request.Participant.LastName, 
                    request.Participant.Email, 
                    request.Participant.CourseName, 
                    request.Participant.CompletionDate), 
                memStream);
            memStream.Position = 0L;
            await memStream.CopyToAsync(stream, cancellationToken);
        }
        
        _logger.LogDebug("Cert done (batch: {0}): {1}", request.BatchId, certId);

        return new GenerateCertificateResult
        {
            CertificateId = certId,
            CertificateUri = $"certificate/{certId}"
        };
    }
}