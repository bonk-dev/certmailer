using CertMailer.CertificateGen.Application.Interfaces;
using CertMailer.Shared.Application.Services;
using MediatR;

namespace CertMailer.CertificateGen.Application.Commands.Templates;

public class GetTemplateImageStreamCommand : IRequest<Stream?>
{
    public required int Id { get; set; }
}

public class GetTemplateImageStreamCommandHandler : IRequestHandler<GetTemplateImageStreamCommand, Stream?>
{
    private readonly ITemplateRepository _repository;
    private readonly IBlobStorage _blobStorage;

    public GetTemplateImageStreamCommandHandler(ITemplateRepository repository, IBlobStorage blobStorage)
    {
        _repository = repository;
        _blobStorage = blobStorage;
    }
    
    public async Task<Stream?> Handle(GetTemplateImageStreamCommand request, CancellationToken cancellationToken)
    {
        var template = await _repository.GetTemplateAsync(request.Id);
        if (template == null || string.IsNullOrEmpty(template.BackgroundUri))
        {
            return null;
        }

        var stream = new MemoryStream();
        await _blobStorage.DownloadAsync(template.BackgroundUri, stream, cancellationToken);

        stream.Position = 0L;
        return stream;
    }
}
