using CertMailer.Shared.Application.Dto;

namespace CertMailer.CertificateGen.Application.Models;

public class JobCertificateResult
{
    public required ParticipantDto Participant { get; set; }
    public required Guid CertificateId { get; set; }
    public required string CertificateUri { get; set; }
}