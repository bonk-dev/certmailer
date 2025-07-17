using System.Collections.Concurrent;
using CertMailer.CertificateGen.Application.Dto;
using CertMailer.CertificateGen.Application.Interfaces;
using CertMailer.CertificateGen.Application.Models;
using CertMailer.Shared.Application.Dto;
using Microsoft.Extensions.Logging;

namespace CertMailer.CertificateGen.Infrastructure.Services;

public class InMemoryJobStorage : IJobStorage
{
    private readonly ConcurrentDictionary<Guid, Job> _jobs = new();
    private readonly ILogger<InMemoryJobStorage> _logger;

    public InMemoryJobStorage(ILogger<InMemoryJobStorage> logger)
    {
        _logger = logger;
    }
    
    public async Task<Job> AddJobAsync(CreateJobDto dto)
    {
        var job = new Job
        {
            BatchId = dto.BatchId,
            ParticipantsDto = dto.Participants,
            MailTemplateId = dto.MailTemplateId,
            SubjectTemplateId = dto.SubjectTemplateId,
            CertificateTemplateId = dto.CertificateTemplateId
        };
        return !_jobs.TryAdd(dto.BatchId, job) 
            ? _jobs[dto.BatchId] 
            : job;
    }

    public Task<Job?> GetJobAsync(Guid guid)
    {
        if (_jobs.TryGetValue(guid, out var job))
        {
            return Task.FromResult<Job?>(job);
        }

        return Task.FromResult<Job?>(null);
    }

    public Task RemoveJobAsync(Guid guid)
    {
        _ = _jobs.TryRemove(guid, out _);
        return Task.CompletedTask;
    }

    public Task UpdateJobAsync(Job job) => 
        Task.CompletedTask;
}