var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureAppConfiguration((hostingContext, configure) =>
{
    configure
    .AddJsonFile($"Configuration.{hostingContext.HostingEnvironment.EnvironmentName}.json")
    .AddEnvironmentVariables();
});

builder.Services.AddHttpClient<TokenExchangeDelegateHandler>();

builder.Services.AddOcelot().AddDelegatingHandler<TokenExchangeDelegateHandler>();

builder.Services.AddAuthentication().AddJwtBearer("GatewayAuthenticationSchema", options =>
{
    options.Authority = builder.Configuration["IdentityServerUrl"];
    options.Audience = "resource_gateway";
    options.RequireHttpsMetadata = false;
});

var app = builder.Build();
app.UseHttpsRedirection();
await app.UseOcelot();

app.Run();
