namespace CertMailer.Shared.Application.Dto;

public class ExcelParsed
{ 
    public required Guid BatchId { get; set; }
    public required ParticipantDto[] Participants { get; set; }
    public int? MailTemplateId { get; set; }
    public int? SubjectTemplateId { get; set; }
    public int? CertificateTemplateId { get; set; }
}