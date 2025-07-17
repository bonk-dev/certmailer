using CertMailer.CertificateGen.Application.Interfaces;
using CertMailer.CertificateGen.Application.Models;
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
    public int? TemplateId { get; set; }
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
    private readonly ITemplateRepository _templateRepository;

    public GenerateCertificateCommandHandler(
        ILogger<GenerateCertificateCommandHandler> logger,
        ICertificateService certificateService,
        IBlobStorage blobStorage,
        ITemplateRepository templateRepository)
    {
        _logger = logger;
        _certificateService = certificateService;
        _blobStorage = blobStorage;
        _templateRepository = templateRepository;
    }
    
    public async Task<GenerateCertificateResult> Handle(GenerateCertificateCommand request, CancellationToken cancellationToken)
    {
        var certId = Guid.NewGuid();
        _logger.LogDebug("Generating cert (batch: {0}): {1}", request.BatchId, certId);

        var options = CertificateOptions.Default;
        using var backgroundStream = new MemoryStream();
        if (request.TemplateId.HasValue)
        {
            var template = await _templateRepository.GetTemplateAsync(request.TemplateId.Value);
            if (template == null)
            {
                throw new Exception("Invalid template id");
            }

            if (!string.IsNullOrEmpty(template.BackgroundUri))
            {
                await _blobStorage.DownloadAsync(template.BackgroundUri, backgroundStream, cancellationToken);
                backgroundStream.Position = 0L;
                options = new CertificateOptions
                {
                    BackgroundDocument = backgroundStream,
                    Title = template.Title,
                    Subtitle = template.Subtitle,
                    DescriptionFormat = template.Description
                };
            }
            else
            {
                options = new CertificateOptions
                {
                    BackgroundDocument = null,
                    Title = template.Title,
                    Subtitle = template.Subtitle,
                    DescriptionFormat = template.Description
                };
            }
        }

        using var memStream = new MemoryStream();
        _certificateService.GeneratePdf(
            new Participant(
                request.Participant.FirstName,
                request.Participant.LastName, 
                request.Participant.Email, 
                request.Participant.CourseName, 
                request.Participant.CompletionDate), 
            memStream, 
            options);
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