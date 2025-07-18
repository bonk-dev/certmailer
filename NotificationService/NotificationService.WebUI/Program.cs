using CertMailer.NotificationService.Application;
using CertMailer.NotificationService.Infrastructure;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services
    .AddOpenApi()
    .AddInfrastructureServices(builder.Configuration)
    .AddApplicationServices();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.MapHangfireDashboard();
app.MapControllers();

app.Run();
