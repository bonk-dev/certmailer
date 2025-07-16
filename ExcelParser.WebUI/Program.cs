using CertMailer.ExcelParser.Application;
using CertMailer.ExcelParser.Application.Commands;
using CertMailer.ExcelParser.Application.Dto;
using CertMailer.ExcelParser.Application.Interfaces;
using CertMailer.ExcelParser.Application.Models;
using CertMailer.ExcelParser.Infrastructure;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddControllers();

builder.Services
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/api/status/{batchId}", async (Guid batchId) =>
{
    await using var scope = app.Services.CreateAsyncScope();
    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
    var job = await mediator.Send(new GetJobCommand
    {
        BatchId = batchId
    });

    string status;
    // TODO: return 404
    if (job.Result != null)
    {
        status = job.Result.Success ? "parsed" : "error";
    }
    else
    {
        status = "uploaded";
    }
    return new JobDto(job.BatchId, status, job.Result?.Errors);
});

app.Run();
