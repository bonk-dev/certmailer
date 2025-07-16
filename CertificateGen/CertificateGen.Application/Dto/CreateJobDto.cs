using CertMailer.Shared.Application.Dto;

namespace CertMailer.CertificateGen.Application.Dto;

public readonly struct CreateJobDto
{
    public required Guid BatchId { get; init; }
    public required ParticipantDto[] Participants { get; init; }
    public int? MailTemplateId { get; init; }
    public int? SubjectTemplateId { get; init; }
}