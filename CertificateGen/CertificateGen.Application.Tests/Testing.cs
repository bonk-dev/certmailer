using CertMailer.CertificateGen.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;

namespace CertMailer.CertificateGen.Application.Tests;

[SetUpFixture]
public class Testing
{
    private static IServiceScopeFactory _scopeFactory = null!;
    
    [OneTimeSetUp]
    public void RunBefore()
    {
        QuestPDF.Settings.License = LicenseType.Community;
        
        var services = new ServiceCollection();
        services.AddInfrastructureServices();
        
        _scopeFactory = services.BuildServiceProvider().GetService<IServiceScopeFactory>()!;
    }
    
    public static T GetRequiredService<T>() where T: notnull
    {
        using var scope = _scopeFactory.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<T>();
        return service;
    }
}