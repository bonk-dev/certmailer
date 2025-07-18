using Microsoft.AspNetCore.Mvc;

namespace CertMailer.CertificateGen.WebUI.Endpoints;

[ApiController]
[Route("status")]
public class StatusEndpoint : ControllerBase
{
    [HttpGet("health")]
    public IActionResult OnGetHealth()
    {
        return Ok(new
        {
            Service = "certificate-gen",
            Status = "healthy"
        });
    }
}