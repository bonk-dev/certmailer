using CertMailer.CertificateGen.Application.Interfaces;
using CertMailer.CertificateGen.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CertMailer.CertificateGen.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services)
    {
        services.AddScoped<ICertificateService, CertificateService>();
        return services;
    }
}