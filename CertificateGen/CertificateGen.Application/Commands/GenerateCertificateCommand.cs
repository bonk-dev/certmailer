using CertMailer.CertificateGen.Application.Interfaces;
using CertMailer.Shared.Application.Dto;
using CertMailer.Shared.Application.Services;
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
    private readonly IBlobStorage _blobStorage;

    public GenerateCertificateCommandHandler(
        ILogger<GenerateCertificateCommandHandler> logger,
        ICertificateService certificateService,
        IBlobStorage blobStorage)
    {
        _logger = logger;
        _certificateService = certificateService;
        _blobStorage = blobStorage;
    }
    
    public async Task<GenerateCertificateResult> Handle(GenerateCertificateCommand request, CancellationToken cancellationToken)
    {
        var certId = Guid.NewGuid();
        _logger.LogDebug("Generating cert (batch: {0}): {1}", request.BatchId, certId);
        
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
        var certUri = await _blobStorage.UploadAsync(
            "certificates", $"{request.BatchId}/{certId}.pdf", memStream, cancellationToken);
            
        _logger.LogDebug("Cert done (batch: {0}): {1}", request.BatchId, certId);

        return new GenerateCertificateResult
        {
            CertificateId = certId,
            CertificateUri = certUri.ToString()
        };
    }
}