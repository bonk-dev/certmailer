using CertMailer.ExcelParser.Application.Interfaces;
using CertMailer.ExcelParser.Application.Models;
using MediatR;

namespace CertMailer.ExcelParser.Application.Commands;

public class GetJobCommand : IRequest<Job?>
{
    public required Guid BatchId { get; set; }
}

public class GetJobCommandHandler : IRequestHandler<GetJobCommand, Job?>
{
    private readonly IJobStorage _jobStorage;

    public GetJobCommandHandler(IJobStorage jobStorage)
    {
        _jobStorage = jobStorage;
    }
    
    public async Task<Job?> Handle(GetJobCommand request, CancellationToken cancellationToken) => 
        await _jobStorage.GetJobAsync(request.BatchId);
}