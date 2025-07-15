namespace CertMailer.Shared.Application.Dto;

public class ExcelParsed
{ 
    public required Guid BatchId { get; set; }
    public required ParticipantDto[] Participants { get; set; }
}