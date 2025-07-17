using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOcelot();

var app = builder.Build();

app.UseRouting();
app.UseDefaultFiles();
app.UseStaticFiles();
await app.UseOcelot();

app.Run();