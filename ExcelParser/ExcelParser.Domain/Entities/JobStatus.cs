namespace ExcelParser.Domain.Entities;

public class JobStatus
{
    public string ParserState { get; set; } = "uploaded";
    public int CertificatesGenerated { get; set; }
    public int MailsSent { get; set; }
}