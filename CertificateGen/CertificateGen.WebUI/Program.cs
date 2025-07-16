using CertMailer.CertificateGen.Application;
using CertMailer.CertificateGen.Application.Commands;
using CertMailer.CertificateGen.Application.Models;
using CertMailer.CertificateGen.Infrastructure;
using CertMailer.Shared.Application.Dto;
using Hangfire;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard();
    app.MapHangfireDashboard();
}

app.UseHttpsRedirection();

app.MapGet("/api/status/{batchId}", async (Guid batchId) =>
{
    await using var scope = app.Services.CreateAsyncScope();
    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
    var job = await mediator.Send(new GetJobCommand
    {
        BatchId = batchId
    });

    if (job == null)
    {
        // TODO: Return 404
        throw new Exception();
    }

    return new JobStatus(job.JobStatus, job.Results);
});

app.Run();

record TestAddJobResult(Guid Job);

record JobStatus(Job.Status Status, IEnumerable<JobCertificateResult> Data);
