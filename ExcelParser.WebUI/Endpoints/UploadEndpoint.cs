using CertMailer.ExcelParser.Application.Commands;
using CertMailer.ExcelParser.Application.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CertMailer.ExcelParser.WebUI.Endpoints;

[ApiController]
[Route("parser/upload")]
public class UploadEndpoint : ControllerBase
{
    private readonly IMediator _mediator;

    public UploadEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("test")]
    public async Task<IActionResult> OnPostTestAsync(
        [FromForm] int? mailTemplateId, [FromForm] int? subjectTemplateId)
    {
        await using var s = System.IO.File.OpenRead(
            "/home/bonk/Programowanie/CertMailer-data/participants_valid.xlsx");
        var command = new AddJobCommand
        {
            FileStream = s,
            MailTemplateId = mailTemplateId,
            SubjectTemplateId = subjectTemplateId
        };
        var addResult = await _mediator.Send(command);
        return Ok(new JobDto(addResult, "uploaded"));
    }
}