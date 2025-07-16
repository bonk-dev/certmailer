using Microsoft.AspNetCore.Mvc;

namespace NotificationService.WebUI.Endpoints;

[ApiController]
[Route("status")]
public class StatusEndpoint : ControllerBase
{
    [HttpGet("health")]
    public IActionResult OnGetHealth()
    {
        return Ok(new
        {
            Service = "notification-service",
            Status = "healthy"
        });
    }
}