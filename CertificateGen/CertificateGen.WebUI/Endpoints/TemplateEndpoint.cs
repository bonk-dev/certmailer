using CertMailer.CertificateGen.Application.Commands.Templates;
using CertMailer.CertificateGen.WebUI.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CertMailer.CertificateGen.WebUI.Endpoints;

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

    [HttpGet("{id}/background")]
    public async Task<IActionResult> OnGetDownloadBackgroundAsync(int id)
    {
        var stream = await _mediator.Send(new GetTemplateImageStreamCommand
        {
            Id = id
        });

        if (stream == null)
        {
            return NotFound();
        }

        // Could also store the content type, but this is simpler
        return File(stream, "application/octet-stream", $"template-{id}");
    }
}