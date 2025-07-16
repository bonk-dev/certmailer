using CertMailer.CertificateGen.Application.Interfaces;
using CertMailer.CertificateGen.Application.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CertMailer.CertificateGen.Application.Commands;

public class SendCertificateEventCommand : IRequest
{
    public required Guid BatchId { get; set; }
    public required JobCertificateResult JobResult { get; set; }
}

public class SendCertificateEventCommandHandler : IRequestHandler<SendCertificateEventCommand>
{
    private readonly ILogger<SendCertificateEventCommandHandler> _logger;
    private readonly IMessageBus _bus;

    public SendCertificateEventCommandHandler(ILogger<SendCertificateEventCommandHandler> logger, IMessageBus bus)
    {
        _logger = logger;
        _bus = bus;
    }
    
    public async Task Handle(SendCertificateEventCommand request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Sending CertificateGenerated event on message bus: {0}", request.BatchId);
        
        await _bus.PublishCertificateGeneratedAsync(
            request.BatchId, 
            request.JobResult.Participant.Email,
            request.JobResult.CertificateId,
            request.JobResult.CertificateUri);
    }
}