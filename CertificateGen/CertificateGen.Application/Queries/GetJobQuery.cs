using CertMailer.CertificateGen.Application.Interfaces;
using CertMailer.CertificateGen.Application.Models;
using MediatR;

namespace CertMailer.CertificateGen.Application.Queries;

public class GetJobQuery : IRequest<Job?>
{
    public required Guid BatchId { get; set; }
}

public class GetJobQueryHandler : IRequestHandler<GetJobQuery, Job?>
{
    private readonly IJobStorage _jobStorage;

    public GetJobQueryHandler(IJobStorage jobStorage)
    {
        _jobStorage = jobStorage;
    }
    
    public async Task<Job?> Handle(GetJobQuery request, CancellationToken cancellationToken) => 
        await _jobStorage.GetJobAsync(request.BatchId);
}