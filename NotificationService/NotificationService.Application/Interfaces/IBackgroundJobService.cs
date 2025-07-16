using System.Linq.Expressions;

namespace CertMailer.NotificationService.Application.Interfaces;

public interface IBackgroundJobService
{
    string Enqueue(Expression<Func<Task>> methodCall);
}