using CertMailer.ExcelParser.Application.Models;

namespace CertMailer.ExcelParser.Application.Interfaces;

public interface IJobStorage
{
    Task<Job> StoreFileAsync(Stream stream);
    Task<Job?> GetJobAsync(Guid guid);
    Task RemoveFileAsync(Guid guid);
    Task UpdateJobAsync(Job job);
}