using CertMailer.CertificateGen.Application.Interfaces;
using CertMailer.CertificateGen.Infrastructure.Bus;
using CertMailer.CertificateGen.Infrastructure.Services;
using CertMailer.Shared.Application.Services;
using Hangfire;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;
using Shared.Infrastructure.Models;
using Shared.Infrastructure.Services;

namespace CertMailer.CertificateGen.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfigurationRoot configuration)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        
        services
            .Configure<RabbitMqTransportOptions>(configuration.GetRequiredSection("RabbitMq"))
            .Configure<FilesystemBlobStorageOptions>(configuration.GetRequiredSection("FilesystemBlobs"));
        services
            .AddSingleton<IJobStorage, InMemoryJobStorage>()
            .AddScoped<ICertificateService, CertificateService>()
            .AddMassTransit(x =>
            {
                x.AddConsumer<ExcelParsedConsumer>();
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.ConfigureEndpoints(ctx);
                });
            })
            .AddScoped<IMessageBus, MassTransitMessageBus>()
            .AddHangfire(cfg =>
            {
                cfg
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseInMemoryStorage();
            })
            .AddHangfireServer()
            .AddSingleton<IBackgroundJobService, BackgroundJobService>()
            .AddScoped<IBlobStorage, FilesystemBlobStorage>();
        return services;
    }
}