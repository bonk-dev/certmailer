using CertMailer.CertificateGen.Application.Commands;
using CertMailer.CertificateGen.Application.Interfaces;
using CertMailer.Shared.Application.Dto;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CertMailer.CertificateGen.Infrastructure.Bus;

public class ExcelParsedConsumer : IConsumer<ExcelParsed>, IMessageBus
{
    private readonly IMediator _mediator;
    private readonly ILogger<MassTransitBus> _logger;

    public ExcelParsedConsumer(IMediator mediator, ILogger<MassTransitBus> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    
    public async Task Consume(ConsumeContext<ExcelParsed> context)
    {
        _logger.LogDebug("Consuming ExcelParsed");
        await _mediator.Send(new AddJobCommand
        {
            ExcelParsedEvent = context.Message
        });
    }
}