using CertMailer.ExcelParser.Application.Dto;
using CertMailer.ExcelParser.Application.Models;

namespace CertMailer.ExcelParser.Application.Interfaces;

public interface IJobStorage
{
    Task<IEnumerable<Job>> GetAllJobsAsync();
    Task<Job> AddJobAsync(JobCreationDto jobCreationDto);
    Task<Job?> GetJobAsync(Guid guid);
    Task RemoveFileAsync(Guid guid);
    Task UpdateJobAsync(Job job);
}