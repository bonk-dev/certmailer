using CertMailer.Application.Interfaces;
using CertMailer.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CertMailer.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IExcelService, ExcelService>();
        return services;
    }
}