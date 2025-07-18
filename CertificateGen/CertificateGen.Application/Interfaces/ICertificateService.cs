using CertMailer.CertificateGen.Application.Models;
using CertMailer.Shared.Domain.Entities;

namespace CertMailer.CertificateGen.Application.Interfaces;

public interface ICertificateService
{
    bool VerifyImage(Stream stream);
    void GeneratePdf(Participant participant, Stream stream);
    void GeneratePdf(Participant participant, Stream stream, CertificateOptions options);
}