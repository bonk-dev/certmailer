using CertMailer.NotificationService.Application.Interfaces;
using CertMailer.NotificationService.Application.Models;
using CertMailer.Shared.Application.Services;
using MediatR;

namespace CertMailer.NotificationService.Application.Commands;

public class SendCertificateEmailCommand : IRequest
{
    public required string To { get; set; }
    public required string Email { get; set; }
    public required Guid CertificateId { get; set; }
    public required string CertificateUri { get; set; }
}

public class SendCertificateEmailCommandHandler : IRequestHandler<SendCertificateEmailCommand>
{
    private readonly IEmailService _emailService;
    private readonly IBackgroundJobService _jobClient;
    private readonly IBlobStorage _blobStorage;

    public SendCertificateEmailCommandHandler(
        IEmailService emailService, 
        IBackgroundJobService jobClient,
        IBlobStorage blobStorage)
    {
        _emailService = emailService;
        _jobClient = jobClient;
        _blobStorage = blobStorage;
    }

    public async Task SendEmailAsync(SendCertificateEmailCommand request, CancellationToken cancellationToken)
    {
        using var stream = new MemoryStream();
        await _blobStorage.DownloadAsync(request.CertificateUri, stream, cancellationToken);
        stream.Position = 0L;
        
        // TODO: Allow custom subject, and mailbody
        await _emailService.SendEmailAsync(new EmailMessageRequest(
            request.Email,
            request.To,
            "Certyfikat", "Gratulacje", [
                new EmailAttachment
                {
                    FileName = request.CertificateId + ".pdf",
                    ContentType = "application/pdf",
                    Data = stream 
                }
            ]));
    }
    
    public Task Handle(SendCertificateEmailCommand request, CancellationToken cancellationToken)
    {
        _jobClient.Enqueue(() => SendEmailAsync(request, cancellationToken));
        return Task.CompletedTask;
    }
}