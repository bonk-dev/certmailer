using CertMailer.CertificateGen.Application.Interfaces;
using CertMailer.CertificateGen.Domain.Entities;
using CertMailer.Shared.Application.Services;
using MediatR;

namespace CertMailer.CertificateGen.Application.Commands.Templates;

public class AddOrUpdateTemplateCommand : IRequest
{
    public int? IdToUpdate { get; set; }
    public required string Name { get; set; }
    public required string Title { get; set; }
    public required string Subtitle { get; set; }
    public required string Description { get; set; }
    public Stream? BackgroundImage { get; set; }
}

public class AddTemplateCommandHandler : IRequestHandler<AddOrUpdateTemplateCommand>
{
    private readonly ITemplateRepository _repository;
    private readonly IBlobStorage _blobStorage;

    public AddTemplateCommandHandler(
        ITemplateRepository repository,
        IBlobStorage blobStorage)
    {
        _repository = repository;
        _blobStorage = blobStorage;
    }
    
    public async Task Handle(AddOrUpdateTemplateCommand request, CancellationToken cancellationToken)
    {
        string? uri = null;
        if (request.BackgroundImage != null)
        {
            var guid = Guid.NewGuid();
            uri = (await _blobStorage.UploadAsync(
                "templates", $"background-{guid}", request.BackgroundImage, cancellationToken))
                .ToString();
        }

        var template = new CertificateTemplate
        {
            Id = request.IdToUpdate ?? 0,
            Name = request.Name,
            Title = request.Title,
            Subtitle = request.Subtitle,
            Description = request.Description,
            BackgroundUri = uri
        };

        if (request.IdToUpdate.HasValue)
        {
            await _repository.UpdateTemplateAsync(template);
        }
        else
        {
            await _repository.AddTemplateAsync(template);
        }
    }
}