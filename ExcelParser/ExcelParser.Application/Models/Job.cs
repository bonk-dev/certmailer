using System.Buffers;
using CertMailer.ExcelParser.Application.Interfaces;
using CertMailer.Shared.Domain.Entities;

namespace CertMailer.ExcelParser.Application.Models;

public class Job
{
    public required Guid BatchId { get; init; }
    public required IMemoryOwner<byte>? Data { get; set; }
    public IResult<IEnumerable<Participant>>? Result { get; set; }
}