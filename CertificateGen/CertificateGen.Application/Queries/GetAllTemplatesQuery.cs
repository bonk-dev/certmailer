using CertMailer.CertificateGen.Application.Interfaces;
using CertMailer.CertificateGen.Domain.Entities;
using MediatR;

namespace CertMailer.CertificateGen.Application.Queries;

public class GetAllTemplatesQuery : IRequest<IEnumerable<CertificateTemplate>>;

public class GetTemplatesQueryHandler : IRequestHandler<GetAllTemplatesQuery, IEnumerable<CertificateTemplate>>
{
    private readonly ITemplateRepository _repository;

    public GetTemplatesQueryHandler(ITemplateRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<IEnumerable<CertificateTemplate>> Handle(GetAllTemplatesQuery request, CancellationToken cancellationToken) => 
        await _repository.GetAllTemplatesAsync();
}