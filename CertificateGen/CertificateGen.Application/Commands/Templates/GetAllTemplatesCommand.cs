using CertMailer.CertificateGen.Application.Interfaces;
using CertMailer.CertificateGen.Domain.Entities;
using MediatR;

namespace CertMailer.CertificateGen.Application.Commands.Templates;

public class GetAllTemplatesCommand : IRequest<IEnumerable<CertificateTemplate>>;

public class GetTemplatesCommandHandler : IRequestHandler<GetAllTemplatesCommand, IEnumerable<CertificateTemplate>>
{
    private readonly ITemplateRepository _repository;

    public GetTemplatesCommandHandler(ITemplateRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<IEnumerable<CertificateTemplate>> Handle(GetAllTemplatesCommand request, CancellationToken cancellationToken) => 
        await _repository.GetAllTemplatesAsync();
}