using CertMailer.NotificationService.Application.Interfaces;
using MediatR;
using NotificationService.Domain.Entities;

namespace CertMailer.NotificationService.Application.Queries;

public class GetAllTemplatesQuery : IRequest<IEnumerable<MailTemplate>>
{
}

public class GetAllTemplatesQueryHandler : IRequestHandler<GetAllTemplatesQuery, IEnumerable<MailTemplate>>
{
    private readonly ITemplateRepository _repository;

    public GetAllTemplatesQueryHandler(ITemplateRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<IEnumerable<MailTemplate>> Handle(GetAllTemplatesQuery request, CancellationToken cancellationToken) => 
        await _repository.GetAllTemplatesAsync();
}