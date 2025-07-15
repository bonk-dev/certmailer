using CertMailer.ExcelParser.Application.Interfaces;
using CertMailer.ExcelParser.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;

namespace CertMailer.ExcelParser.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services)
    {
        ExcelPackage.License.SetNonCommercialPersonal("Dawid Pągowski");
        services.AddScoped<IExcelService, ExcelService>();
        services.AddSingleton<IJobStorage, InMemoryJobStorage>();
        return services;
    }
}