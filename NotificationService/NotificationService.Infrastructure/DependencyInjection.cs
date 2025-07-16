using CertMailer.NotificationService.Application.Interfaces;
using CertMailer.NotificationService.Application.Models.Settings;
using CertMailer.NotificationService.Infrastructure.Consumers;
using CertMailer.NotificationService.Infrastructure.Services;
using CertMailer.Shared.Application.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Infrastructure.Models;
using Shared.Infrastructure.Services;

namespace CertMailer.NotificationService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services
            .Configure<MailSettings>(configuration.GetRequiredSection("MailSettings"))
            .Configure<RabbitMqTransportOptions>(configuration.GetRequiredSection("RabbitMq"));
            .Configure<RabbitMqTransportOptions>(configuration.GetRequiredSection("RabbitMq"))
            .Configure<FilesystemBlobStorageOptions>(configuration.GetRequiredSection("FilesystemBlobs"));
        services
            .AddScoped<IEmailService, SmtpEmailService>()
            .AddMassTransit(x =>
            {
                x.AddConsumer<CertificateGeneratedConsumer>();
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.ConfigureEndpoints(ctx);
                });
            });
            .AddScoped<IBlobStorage, FilesystemBlobStorage>();
        return services;
    }
}