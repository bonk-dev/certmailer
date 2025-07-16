using CertMailer.Shared.Application.Dto;

namespace CertMailer.ExcelParser.Application.Dto;

public class ExcelParsedDto
{
    public required Guid BatchId { get; set; }
    public required ParticipantDto[] Participants { get; set; }
    public int? MailTemplateId { get; set; }
    public int? SubjectTemplateId { get; set; }
}