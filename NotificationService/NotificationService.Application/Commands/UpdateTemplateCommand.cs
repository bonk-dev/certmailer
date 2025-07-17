using CertMailer.NotificationService.Application.Interfaces;
using MediatR;
using NotificationService.Domain.Entities;

namespace CertMailer.NotificationService.Application.Commands;

public class UpdateTemplateCommand : IRequest
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Template { get; set; }
}

public class UpdateTemplateCommandHandler : IRequestHandler<UpdateTemplateCommand>
{
    private readonly ITemplateRepository _repository;

    public UpdateTemplateCommandHandler(ITemplateRepository repository)
    {
        _repository = repository;
    }
    
    public async Task Handle(UpdateTemplateCommand request, CancellationToken cancellationToken)
    {
        await _repository.UpdateTemplateAsync(new MailTemplate
        {
            Id = request.Id,
            Name = request.Name,
            Template = request.Template
        });
    }
}