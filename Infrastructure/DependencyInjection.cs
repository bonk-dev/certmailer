using CertMailer.Application.Interfaces;
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
        services.AddSingleton<IExcelService, ExcelService>();
        services.AddScoped<ICertificateService, CertificateService>();
        return services;
    }
}