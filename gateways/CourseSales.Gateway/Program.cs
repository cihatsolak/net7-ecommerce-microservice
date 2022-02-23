var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureAppConfiguration((hostingContext, configure) =>
{
    configure
    .AddJsonFile($"Configuration.{hostingContext.HostingEnvironment.EnvironmentName}.json")
    .AddEnvironmentVariables();
});

builder.Services.AddOcelot();

var app = builder.Build();
app.UseHttpsRedirection();
await app.UseOcelot();

app.Run();
