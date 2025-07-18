using System.ComponentModel.DataAnnotations;

namespace CertMailer.NotificationService.WebUI.Models;

public record UpdateTemplateRequest(
    [param: Required] string Name, 
    [param: Required] string Template);