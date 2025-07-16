using System.Linq.Expressions;

namespace CertMailer.CertificateGen.Application.Interfaces;

public interface IBackgroundJobService
{
    string Enqueue(Expression<Func<Task>> methodCall);
}