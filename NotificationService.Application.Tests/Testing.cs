using CertMailer.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using QuestPDF.Infrastructure;

namespace CertMailer.Application.Tests;

[SetUpFixture]
public class Testing
{
    private static IConfigurationRoot _configuration = null!;
    private static IServiceScopeFactory _scopeFactory = null!;
    
    [OneTimeSetUp]
    public void RunBefore()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true);
        _configuration = builder.Build();
        
        var services = new ServiceCollection();
        services.AddInfrastructureServices(_configuration);
        
        _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>()!;
    }
    
    public static T GetRequiredService<T>() where T: notnull
    {
        using var scope = _scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<T>();
        return service;
    }
}