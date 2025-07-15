using CertMailer.ExcelParser.Application.Interfaces;
using CertMailer.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CertMailer.ExcelParser.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services)
    {
        services.AddScoped<IExcelService, ExcelService>();
        return services;
    }
}