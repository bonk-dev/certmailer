using CertMailer.Shared.Domain.Entities;

namespace CertMailer.ExcelParser.Application.Interfaces;

public interface IExcelService
{
    Task<IResult<IEnumerable<Participant>>> ParseAsync(Stream stream);
    Task<IResult<IEnumerable<Participant>>> ParseAsync(Stream stream, string sheetName);
    IResult<IEnumerable<Participant>> Parse(Memory<byte> buffer, string sheetName);
}