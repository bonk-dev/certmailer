namespace CertMailer.ExcelParser.Application.Dto;

public record JobDto(Guid BatchId, string Status, IEnumerable<string>? Errors = null);