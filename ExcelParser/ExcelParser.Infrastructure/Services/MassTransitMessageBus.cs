using CertMailer.ExcelParser.Application.Interfaces;
using CertMailer.Shared.Application.Dto;
using CertMailer.Shared.Domain.Entities;
using MassTransit;

namespace CertMailer.ExcelParser.Infrastructure.Services;

public class MassTransitMessageBus : IMessageBus
{
    private readonly IPublishEndpoint _endpoint;

    public MassTransitMessageBus(IPublishEndpoint endpoint)
    {
        _endpoint = endpoint;
    }
    
    public async Task PublishExcelParsedAsync(Guid batchId, IEnumerable<Participant> participants)
    {
        await _endpoint.Publish(new ExcelParsed
        {
            BatchId = batchId,
            Participants = participants.Select(p => new ParticipantDto
            {
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                CourseName = p.CourseName,
                CompletionDate = p.CompletionDate
            }).ToArray()
        });
    }
}