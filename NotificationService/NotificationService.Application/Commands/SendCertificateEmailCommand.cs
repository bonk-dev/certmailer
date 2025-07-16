using CertMailer.NotificationService.Application.Interfaces;
using CertMailer.NotificationService.Application.Models;
using CertMailer.Shared.Application.Dto;
using CertMailer.Shared.Application.Services;
using MediatR;
using NotificationService.Domain.Entities;

namespace CertMailer.NotificationService.Application.Commands;

public class SendCertificateEmailCommand : IRequest
{
    public required ParticipantDto Participant { get; set; }
    public required Guid CertificateId { get; set; }
    public required string CertificateUri { get; set; }
    public int? MailTemplateId { get; set; }
}

public class SendCertificateEmailCommandHandler : IRequestHandler<SendCertificateEmailCommand>
{
    private readonly IEmailService _emailService;
    private readonly IBackgroundJobService _jobClient;
    private readonly IBlobStorage _blobStorage;
    private readonly ITemplateRepository _templateRepository;
    private readonly ITemplateEngine _templateEngine;

    public SendCertificateEmailCommandHandler(
        IEmailService emailService, 
        IBackgroundJobService jobClient,
        IBlobStorage blobStorage,
        ITemplateRepository templateRepository,
        ITemplateEngine templateEngine)
    {
        _emailService = emailService;
        _jobClient = jobClient;
        _blobStorage = blobStorage;
        _templateRepository = templateRepository;
        _templateEngine = templateEngine;
    }

    public async Task SendEmailAsync(SendCertificateEmailCommand request, CancellationToken cancellationToken)
    {
        var mailBody = await ApplyTemplateAsync(request, request.MailTemplateId);

        using var stream = new MemoryStream();
        await _blobStorage.DownloadAsync(request.CertificateUri, stream, cancellationToken);
        stream.Position = 0L;
        
        // TODO: Allow custom subject
        await _emailService.SendEmailAsync(new EmailMessageRequest(
            request.Participant.Email,
            $"{request.Participant.FirstName} {request.Participant.LastName}",
            "Certyfikat", mailBody, [
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
    
    private async Task<string> ApplyTemplateAsync(SendCertificateEmailCommand request, int? templateId)
    {
        MailTemplate template;
        if (templateId.HasValue)
        {
            var templateTemp = await _templateRepository.GetTemplateAsync(templateId.Value);
            template = templateTemp 
                       ?? throw new Exception($"Template with id {templateId.Value} does not exist");
        }
        else
        {
            template = await _templateRepository.GetDefaultTemplateAsync();
        }

        var mailBody = _templateEngine.ApplyTemplate(template, new Dictionary<string, string>()
        {
            { MailTemplateKeys.FirstName, request.Participant.FirstName },
            { MailTemplateKeys.CourseName, request.Participant.CourseName }
        });
        return mailBody;
    }
}