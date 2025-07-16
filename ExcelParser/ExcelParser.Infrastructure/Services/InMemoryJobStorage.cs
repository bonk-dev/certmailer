using System.Buffers;
using CertMailer.ExcelParser.Application.Dto;
using CertMailer.ExcelParser.Application.Interfaces;
using CertMailer.ExcelParser.Application.Models;
using Microsoft.Extensions.Logging;

namespace CertMailer.ExcelParser.Infrastructure.Services;

public class InMemoryJobStorage : IJobStorage
{
    private readonly ILogger<InMemoryJobStorage> _logger;
    private readonly Dictionary<Guid, Job> _storedFiles = new();

    public InMemoryJobStorage(ILogger<InMemoryJobStorage> logger)
    {
        _logger = logger;
    }
    
    public async Task<Job> AddJobAsync(JobCreationDto jobCreationDto)
    {
        var guid = Guid.NewGuid();
        _logger.LogDebug("Storing file {0}", guid);

        var buffer = MemoryPool<byte>.Shared.Rent((int)jobCreationDto.Stream.Length);
        _ = await jobCreationDto.Stream.ReadAsync(buffer.Memory);

        var job = new Job
        {
            BatchId = guid,
            Data = buffer,
            MailTemplateId = jobCreationDto.MailTemplateId,
            SubjectTemplateId = jobCreationDto.SubjectTemplateId,
            Result = null
        };
        _storedFiles.Add(guid, job);
        return job;
    }

    public Task<Job?> GetJobAsync(Guid guid)
    {
        return _storedFiles.TryGetValue(guid, out var job) 
            ? Task.FromResult<Job?>(job) 
            : Task.FromResult<Job?>(null);
    }

    public Task RemoveFileAsync(Guid guid)
    {
        _logger.LogDebug("Removing file {0}", guid);
        
        if (_storedFiles.TryGetValue(guid, out var owner))
        {
            owner.Data!.Dispose();
            _storedFiles.Remove(guid);
        }

        return Task.CompletedTask;
    }

    public Task UpdateJobAsync(Job job) => Task.CompletedTask;
}