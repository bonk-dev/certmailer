using CertMailer.NotificationService.Application.Interfaces;
using MediatR;
using NotificationService.Domain.Entities;

namespace CertMailer.NotificationService.Application.Commands;

public class GetAllTemplatesCommand : IRequest<IEnumerable<MailTemplate>>
{
}

public class GetAllTemplatesCommandHandler : IRequestHandler<GetAllTemplatesCommand, IEnumerable<MailTemplate>>
{
    private readonly ITemplateRepository _repository;

    public GetAllTemplatesCommandHandler(ITemplateRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<IEnumerable<MailTemplate>> Handle(GetAllTemplatesCommand request, CancellationToken cancellationToken) => 
        await _repository.GetAllTemplatesAsync();
}