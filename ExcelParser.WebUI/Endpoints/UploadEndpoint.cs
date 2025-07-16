using CertMailer.ExcelParser.Application.Commands;
using CertMailer.ExcelParser.Application.Dto;
using ExcelParser.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CertMailer.ExcelParser.WebUI.Endpoints;

[ApiController]
[Route("upload")]
public class UploadEndpoint : ControllerBase
{
    private readonly IMediator _mediator;

    public UploadEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpPost("")]
    public async Task<IActionResult> OnPostUploadAsync(
        List<IFormFile> files,
        [FromForm] int? mailTemplateId, [FromForm] int? subjectTemplateId)
    {
        if (files.Count > 1)
        {
            return BadRequest(new
            {
                Error = "Only single files are allowed"
            });
        }

        if (files.Count < 1)
        {
            return BadRequest(new
            {
                Error = "A file is required"
            });
        }

        var command = new AddJobCommand
        {
            FileStream = files[0].OpenReadStream(),
            MailTemplateId = mailTemplateId,
            SubjectTemplateId = subjectTemplateId
        };
        var addResult = await _mediator.Send(command);
        return Ok(new JobDto(addResult, new JobStatus()
        {
            // TODO
        }));
    }
}