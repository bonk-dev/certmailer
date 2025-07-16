using CertMailer.CertificateGen.Application.Interfaces;
using CertMailer.CertificateGen.Infrastructure.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;

namespace CertMailer.CertificateGen.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfigurationRoot configuration)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        
        services.Configure<RabbitMqTransportOptions>(configuration.GetRequiredSection("RabbitMq"));
        services
            .AddScoped<ICertificateService, CertificateService>()
            .AddMassTransit(x =>
            {
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    
                });
            });
        return services;
    }
}