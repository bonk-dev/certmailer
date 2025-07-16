using CertMailer.NotificationService.Application.Interfaces;
using CertMailer.NotificationService.Application.Models;
using CertMailer.Shared.Domain.Entities;

namespace CertMailer.NotificationService.Application.Tests.Services;

public class MailServiceTests
{
    private static readonly Participant SampleParticipant = new(
        "Aleksander", "Pietrzak",
        "aleksander.pietrzak@poczta.fm",
        "Programowanie Java", new DateTime(2024, 10, 8));
    
    [Test]
    public async Task TestSendEmailAsync()
    {
        var service = Testing.GetRequiredService<IEmailService>();

        var fakeCertificateBuffer = "not really a certificate"u8.ToArray();
        using var memoryStream = new MemoryStream(fakeCertificateBuffer);
        var attachment = new EmailAttachment
        {
            FileName = $"{SampleParticipant.FirstName}_{SampleParticipant.LastName}.txt",
            ContentType = "text/plain",
            Data = memoryStream
        };
        var request = new EmailMessageRequest(
            SampleParticipant.Email,
            $"{SampleParticipant.FirstName} {SampleParticipant.LastName}",
            "Certyfikat ukończenia kursu",
            $"""
            <p>Cześć {SampleParticipant.FirstName}!<br>
            Gratulujemy ukończenia kursu {SampleParticipant.CourseName}.<br>
            W załączniku znajdziesz swój certyfikat.</p>
            
            <p>Pozdrawiamy,<br>
            dpago.dev</p>
            """,
            [attachment]);
        await service.SendEmailAsync(request);
    }
}