using CertMailer.NotificationService.Application.Interfaces;
using CertMailer.NotificationService.Application.Models.Settings;
using CertMailer.NotificationService.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CertMailer.NotificationService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.Configure<MailSettings>(configuration.GetRequiredSection("MailSettings"));
        services.AddScoped<IEmailService, SmtpEmailService>();
        return services;
    }
}