using CertMailer.Shared.Domain.Entities;

namespace CertMailer.ExcelParser.Application.Interfaces;

public interface IMessageBus
{
    Task PublishExcelParsedAsync(Guid batchId, IEnumerable<Participant> participants);
}