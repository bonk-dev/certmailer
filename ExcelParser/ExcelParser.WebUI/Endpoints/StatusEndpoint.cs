using CertMailer.ExcelParser.Application.Commands;
using CertMailer.ExcelParser.Application.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CertMailer.ExcelParser.WebUI.Endpoints;

[ApiController]
[Route("status")]
public class StatusEndpoint : ControllerBase
{
    private readonly IMediator _mediator;

    public StatusEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("health")]
    public IActionResult OnGetHealth()
    {
        return Ok(new
        {
            Service = "excel-parser",
            Status = "healthy"
        });
    }

    [HttpGet("{batchId}")]
    public async Task<IActionResult> OnGetJobStatusAsync(Guid batchId)
    {
        var job = await _mediator.Send(new GetJobCommand
        {
            BatchId = batchId
        });

        if (job == null)
        {
            return NotFound();
        } 

        string status;
        if (job.Result != null)
        {
            status = job.Result.Success ? "parsed" : "error";
        }
        else
        {
            status = "uploaded";
        }
        return Ok(new JobDto(job.BatchId, job.JobStatus, job.Result?.Errors));
    }

    [HttpGet("all")]
    public async Task<IActionResult> OnGetAllJobsAsync()
    {
        var jobs = await _mediator.Send(new GetAllJobsCommand());
        return Ok(jobs.Select(j => new JobDto(j.BatchId, j.JobStatus, j.Result?.Errors)));
    }
}