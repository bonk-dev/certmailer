using CertMailer.ExcelParser.Application.Interfaces;
using CertMailer.ExcelParser.Application.Models;
using MediatR;

namespace CertMailer.ExcelParser.Application.Queries;

public class GetAllJobsQuery : IRequest<IEnumerable<Job>>;

public class GetAllJobsQueryHandler : IRequestHandler<GetAllJobsQuery, IEnumerable<Job>>
{
    private readonly IJobStorage _jobStorage;

    public GetAllJobsQueryHandler(IJobStorage jobStorage)
    {
        _jobStorage = jobStorage;
    }

    public async Task<IEnumerable<Job>> Handle(GetAllJobsQuery request, CancellationToken cancellationToken) => 
        await _jobStorage.GetAllJobsAsync();
}