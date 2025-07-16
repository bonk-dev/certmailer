using CertMailer.CertificateGen.Application.Dto;
using CertMailer.CertificateGen.Application.Models;

namespace CertMailer.CertificateGen.Application.Interfaces;

public interface IJobStorage
{
    Task<Job> AddJobAsync(CreateJobDto dto);
    Task<Job?> GetJobAsync(Guid guid);
    Task RemoveJobAsync(Guid guid);
    Task UpdateJobAsync(Job job);
}