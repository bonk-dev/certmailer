using Microsoft.AspNetCore.Mvc;

namespace CertMailer.ExcelParser.WebUI.Endpoints;

[ApiController]
[Route("parser/upload")]
public class UploadEndpoint : ControllerBase
{
    public UploadEndpoint()
    {
    }

    [HttpPost]
    public async Task<IActionResult> OnPostUploadAsync(List<IFormFile> files)
    {
        throw new NotImplementedException();
    }
}