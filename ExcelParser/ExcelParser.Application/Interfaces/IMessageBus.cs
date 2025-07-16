using CertMailer.ExcelParser.Application.Dto;

namespace CertMailer.ExcelParser.Application.Interfaces;

public interface IMessageBus
{
    Task PublishExcelParsedAsync(ExcelParsedDto excelParsedDto);
}