var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOcelot();

var app = builder.Build();

await app.UseOcelot();

app.Run();
