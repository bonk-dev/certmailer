using CertMailer.CertificateGen.Application.Interfaces;
using CertMailer.CertificateGen.Domain.Entities;
using CertMailer.Shared.Application.Services;
using MediatR;

namespace CertMailer.CertificateGen.Application.Commands.Templates;

public class AddOrUpdateTemplateCommand : IRequest<bool>
{
    public int? IdToUpdate { get; set; }
    public required string Name { get; set; }
    public required string Title { get; set; }
    public required string Subtitle { get; set; }
    public required string Description { get; set; }
    public Stream? BackgroundImage { get; set; }
}

public class AddTemplateCommandHandler : IRequestHandler<AddOrUpdateTemplateCommand, bool>
{
    private readonly ITemplateRepository _repository;
    private readonly IBlobStorage _blobStorage;
    private readonly ICertificateService _certificateService;

    public AddTemplateCommandHandler(
        ITemplateRepository repository,
        IBlobStorage blobStorage,
        ICertificateService certificateService)
    {
        _repository = repository;
        _blobStorage = blobStorage;
        _certificateService = certificateService;
    }
    
    public async Task<bool> Handle(AddOrUpdateTemplateCommand request, CancellationToken cancellationToken)
    {
        string? uri = null;
        if (request.BackgroundImage != null)
        {
            Stream? s = null;

            try
            {
                if (!request.BackgroundImage.CanSeek)
                {
                    s = new MemoryStream(new byte[request.BackgroundImage.Length]);
                    await request.BackgroundImage.CopyToAsync(s, cancellationToken);
                }
                else
                {
                    s = request.BackgroundImage;
                }

                if (!_certificateService.VerifyImage(s))
                {
                    return false;
                }

                s.Seek(0, SeekOrigin.Begin);

                var guid = Guid.NewGuid();
                uri = (await _blobStorage.UploadAsync(
                        "templates", $"background-{guid}", request.BackgroundImage, cancellationToken))
                    .ToString();
            }
            finally
            {
                if (s != null)
                {
                    await s.DisposeAsync();
                }
            }
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

        return true;
    }
}