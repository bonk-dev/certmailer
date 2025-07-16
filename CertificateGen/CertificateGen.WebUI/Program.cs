using CertMailer.CertificateGen.Application;
using CertMailer.CertificateGen.Application.Commands;
using CertMailer.CertificateGen.Infrastructure;
using CertMailer.Shared.Application.Dto;
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
}

app.UseHttpsRedirection();

app.MapPost("/testAddJob", async () =>
{
    await using var scope = app.Services.CreateAsyncScope();
    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
    var jobGuid = await mediator.Send(new AddJobCommand
    {
        ExcelParsedEvent = new ExcelParsed
        {
            BatchId = Guid.NewGuid(),
            Participants =
            [
                new()
                {
                    FirstName = "Aleksander",
                    LastName = "Pietrzak",
                    Email = "aleksander.pietrzak@poczta.fm",
                    CourseName = "Programowanie Java",
                    CompletionDate = new DateTime(2024, 10, 8)
                }
            ]
        }
    });

    return new TestAddJobResult(jobGuid);
});

app.Run();

record TestAddJobResult(Guid Job);
