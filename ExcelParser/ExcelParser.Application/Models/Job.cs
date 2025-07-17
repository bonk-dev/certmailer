using System.Buffers;
using CertMailer.ExcelParser.Application.Interfaces;
using CertMailer.Shared.Domain.Entities;
using ExcelParser.Domain.Entities;

namespace CertMailer.ExcelParser.Application.Models;

public class Job
{
    public required Guid BatchId { get; init; }
    public required IMemoryOwner<byte>? Data { get; set; }
    public int? MailTemplateId { get; init; }
    public int? SubjectTemplateId { get; init; }
    public int? CertificateTemplateId { get; init; }
    public IResult<IEnumerable<Participant>>? Result { get; set; }

    public JobStatus JobStatus { get; set; } = new();
}