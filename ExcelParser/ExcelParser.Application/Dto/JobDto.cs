using ExcelParser.Domain.Entities;

namespace CertMailer.ExcelParser.Application.Dto;

public record JobDto(Guid BatchId, JobStatus Status, IEnumerable<string>? Errors = null);