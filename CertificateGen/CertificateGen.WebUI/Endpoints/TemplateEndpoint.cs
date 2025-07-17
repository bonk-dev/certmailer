using CertificateGen.WebUI.Models;
using CertMailer.CertificateGen.Application.Commands.Templates;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CertificateGen.WebUI.Endpoints;

[ApiController]
[Route("templates")]
public class TemplateEndpoint : ControllerBase
{
    private readonly IMediator _mediator;

    public TemplateEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("all")]
    public async Task<IActionResult> OnGetAllTemplatesAsync() => 
        Ok(await _mediator.Send(new GetAllTemplatesCommand()));

    [HttpPost("")]
    public async Task<IActionResult> OnPostAddTemplateAsync([FromForm] AddTemplateRequest request)
    {
        if (request.BackgroundFile != null)
        {
            await using var stream = request.BackgroundFile?.OpenReadStream();
            var command = new AddOrUpdateTemplateCommand
            {
                Name = request.Name,
                Title = request.Title,
                Subtitle = request.Subtitle,
                Description = request.Description,
                BackgroundImage = stream
            };
            await _mediator.Send(command);
            return Ok();
        }
        else
        {
            var command = new AddOrUpdateTemplateCommand
            {
                Name = request.Name,
                Title = request.Title,
                Subtitle = request.Subtitle,
                Description = request.Description
            };
            await _mediator.Send(command);
            return Ok();
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> OnPutAddTemplateAsync(int id, [FromForm] AddTemplateRequest request)
    {
        if (request.BackgroundFile != null)
        {
            await using var stream = request.BackgroundFile?.OpenReadStream();
            var command = new AddOrUpdateTemplateCommand
            {
                IdToUpdate = id,
                Name = request.Name,
                Title = request.Title,
                Subtitle = request.Subtitle,
                Description = request.Description,
                BackgroundImage = stream
            };
            await _mediator.Send(command);
            return Ok();
        }
        else
        {
            var command = new AddOrUpdateTemplateCommand
            {
                IdToUpdate = id,
                Name = request.Name,
                Title = request.Title,
                Subtitle = request.Subtitle,
                Description = request.Description
            };
            await _mediator.Send(command);
            return Ok();
        }
    }
}