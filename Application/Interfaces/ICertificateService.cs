using CertMailer.Application.Models;
using CertMailer.Domain.Entities;

namespace CertMailer.Application.Interfaces;

public interface ICertificateService
{
    void GeneratePdf(Participant participant, Stream stream);
    void GeneratePdf(Participant participant, Stream stream, CertificateOptions options);
}