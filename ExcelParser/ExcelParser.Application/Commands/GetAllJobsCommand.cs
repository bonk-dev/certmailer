using CertMailer.ExcelParser.Application.Interfaces;
using CertMailer.ExcelParser.Application.Models;
using MediatR;

namespace CertMailer.ExcelParser.Application.Commands;

public class GetAllJobsCommand : IRequest<IEnumerable<Job>>;

public class GetAllJobsCommandHandler : IRequestHandler<GetAllJobsCommand, IEnumerable<Job>>
{
    private readonly IJobStorage _jobStorage;

    public GetAllJobsCommandHandler(IJobStorage jobStorage)
    {
        _jobStorage = jobStorage;
    }

    public async Task<IEnumerable<Job>> Handle(GetAllJobsCommand request, CancellationToken cancellationToken) => 
        await _jobStorage.GetAllJobsAsync();
}