using CertMailer.ExcelParser.Application.Interfaces;
using CertMailer.ExcelParser.Infrastructure.Consumers;
using CertMailer.ExcelParser.Infrastructure.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;

namespace CertMailer.ExcelParser.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfigurationRoot configuration)
    {
        ExcelPackage.License.SetNonCommercialPersonal("Dawid Pągowski");
        services.Configure<RabbitMqTransportOptions>(configuration.GetRequiredSection("RabbitMq"));
        services
            .AddScoped<IExcelService, ExcelService>()
            .AddSingleton<IJobStorage, InMemoryJobStorage>()
            .AddMassTransit(x =>
            {
                x.AddConsumer<EmailSentConsumer>();
                x.AddConsumer<CertificateGeneratedConsumer>();
                
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.ConfigureEndpoints(ctx);
                });
            })
            .AddScoped<IMessageBus, MassTransitMessageBus>();
        return services;
    }
}