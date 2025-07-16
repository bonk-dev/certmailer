namespace CertMailer.ExcelParser.Application.Dto;

public readonly struct JobCreationDto
{
    public Stream Stream { get; init; }
    public int? MailTemplateId { get; init; }
    public int? SubjectTemplateId { get; init; }
}