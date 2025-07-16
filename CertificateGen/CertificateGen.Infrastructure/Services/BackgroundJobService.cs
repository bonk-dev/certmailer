using System.Linq.Expressions;
using CertMailer.CertificateGen.Application.Interfaces;
using Hangfire;

namespace CertMailer.CertificateGen.Infrastructure.Services;

public class BackgroundJobService : IBackgroundJobService
{
    private readonly IBackgroundJobClient _client;

    public BackgroundJobService(IBackgroundJobClient client)
    {
        _client = client;
    }
    
    public string Enqueue(Expression<Func<Task>> methodCall) => 
        _client.Enqueue(methodCall);
}