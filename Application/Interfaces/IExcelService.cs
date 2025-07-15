using CertMailer.Domain.Entities;

namespace CertMailer.Application.Interfaces;

public interface IExcelService
{
    Task<IResult<IEnumerable<Participant>>> ParseAsync(Stream stream);
    Task<IResult<IEnumerable<Participant>>> ParseAsync(Stream stream, string sheetName);
}