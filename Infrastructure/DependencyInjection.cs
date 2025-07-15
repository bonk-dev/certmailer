using CertMailer.Application.Interfaces;
using CertMailer.Application.Models.Settings;
using CertMailer.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CertMailer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddScoped<ICertificateService, CertificateService>();

        services.Configure<MailSettings>(configuration.GetRequiredSection("MailSettings"));
        services.AddScoped<IEmailService, SmtpEmailService>();
        return services;
    }
}