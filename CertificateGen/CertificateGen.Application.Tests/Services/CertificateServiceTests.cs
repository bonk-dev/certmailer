using System.Diagnostics;
using CertMailer.CertificateGen.Application.Interfaces;
using CertMailer.CertificateGen.Application.Models;
using CertMailer.Shared.Domain.Entities;

namespace CertMailer.CertificateGen.Application.Tests.Services;

public class CertificateServiceTests
{
    private static readonly Participant SampleParticipant = new(
        "Aleksander", "Pietrzak",
        "aleksander.pietrzak@poczta.fm",
        "Programowanie Java", new DateTime(2024, 10, 8));
    
    [Test]
    public async Task TestGeneratePdfStream()
    {
        var service = Testing.GetRequiredService<ICertificateService>();
        var memoryStream = new MemoryStream();
        service.GeneratePdf(SampleParticipant, memoryStream, CertificateOptions.Default);

        var tmpPath = Path.GetTempFileName();
        Debug.WriteLine(format: "Writing test PDF to {0}", tmpPath);
        
        await using var f = File.OpenWrite(tmpPath);
        memoryStream.Position = 0L;
        await memoryStream.CopyToAsync(f);
    }
}