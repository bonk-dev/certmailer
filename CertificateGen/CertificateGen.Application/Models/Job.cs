using CertMailer.Shared.Application.Dto;

namespace CertMailer.CertificateGen.Application.Models;

public class Job
{
    public required Guid BatchId { get; set; }
    public Status JobStatus { get; set; } = Status.InProgress;
    public required ParticipantDto[] ParticipantsDto { get; set; }
    public List<JobCertificateResult> Results { get; } = [];
    
    public enum Status
    {
        InProgress,
        Failed,
        Done
    }
}