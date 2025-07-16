using CertMailer.Shared.Application.Dto;

namespace CertMailer.CertificateGen.Application.Dto;

public class CertificateGeneratedDto
{
    public required Guid BatchId { get; set; }
    public required ParticipantDto Participant { get; set; }
    public required CertificateInfoDto Certificate { get; set; }
    public int? MailTemplateId { get; set; }
    public int? SubjectTemplateId { get; set; }
}