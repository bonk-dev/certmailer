using CertMailer.CertificateGen.Application.Models;
using CertMailer.Shared.Application.Dto;

namespace CertMailer.CertificateGen.Application.Interfaces;

public interface IJobStorage
{
    Task<Job> AddJobAsync(Guid batchId, ParticipantDto[] participants);
    Task<Job?> GetJobAsync(Guid guid);
    Task RemoveJobAsync(Guid guid);
    Task UpdateJobAsync(Job job);
}