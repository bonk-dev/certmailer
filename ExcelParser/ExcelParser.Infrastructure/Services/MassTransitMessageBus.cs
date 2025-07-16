using CertMailer.ExcelParser.Application.Dto;
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
    
    public async Task PublishExcelParsedAsync(ExcelParsedDto excelParsedDto)
    {
        await _endpoint.Publish(new ExcelParsed
        {
            BatchId = excelParsedDto.BatchId,
            Participants = excelParsedDto.Participants,
            MailTemplateId = excelParsedDto.MailTemplateId,
            SubjectTemplateId = excelParsedDto.SubjectTemplateId
        });
    }
}