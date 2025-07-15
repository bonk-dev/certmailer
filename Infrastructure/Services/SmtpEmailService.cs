using CertMailer.Application.Interfaces;
using CertMailer.Application.Models;
using CertMailer.Application.Models.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace CertMailer.Infrastructure.Services;

public class SmtpEmailService : IEmailService
{
    private readonly MailSettings _settings;

    public SmtpEmailService(IOptions<MailSettings> options)
    {
        _settings = options.Value;
    }
    
    public async Task SendEmailAsync(EmailMessageRequest request)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.FromDisplayName, _settings.FromAddress));
        message.To.Add(new MailboxAddress(request.RecipientName, request.RecipientEmail));
        message.Subject = request.Subject;
        
        // TODO: Attachments
        var builder = new BodyBuilder();
        builder.HtmlBody = request.MailBody;
        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        // TODO: SSL options
        await client
            .ConnectAsync(_settings.Host, _settings.Port)
            .ConfigureAwait(false);
        await client
            .AuthenticateAsync(_settings.AuthUsername, _settings.AuthPassword)
            .ConfigureAwait(false);
        await client
            .SendAsync(message)
            .ConfigureAwait(false);
        await client
            .DisconnectAsync(quit: true)
            .ConfigureAwait(false);
    }
}
