using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOcelot();

var app = builder.Build();

app.UseRouting();
await app.UseOcelot();

app.Run();