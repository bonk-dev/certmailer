using CertMailer.NotificationService.Application.Commands;
using CertMailer.NotificationService.WebUI.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CertMailer.NotificationService.WebUI.Endpoints;

[ApiController]
[Route("template")]
public class TemplateEndpoint : ControllerBase
{
    private readonly IMediator _mediator;

    public TemplateEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllTemplatesAsync()
    {
        var command = new GetAllTemplatesCommand();
        return Ok(await _mediator.Send(command));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> OnPostSaveTemplateAsync(int id, [FromBody] UpdateTemplateRequest request)
    {
        var command = new UpdateTemplateCommand
        {
            Id = id,
            Name = request.Name,
            Template = request.Template
        };
        await _mediator.Send(command);
        return Ok();
    }
}