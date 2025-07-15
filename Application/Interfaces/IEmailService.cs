using CertMailer.Application.Models;

namespace CertMailer.Application.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessageRequest request);
}