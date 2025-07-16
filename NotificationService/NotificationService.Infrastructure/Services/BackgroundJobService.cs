using System.Linq.Expressions;
using CertMailer.NotificationService.Application.Interfaces;
using Hangfire;

namespace CertMailer.NotificationService.Infrastructure.Services;

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