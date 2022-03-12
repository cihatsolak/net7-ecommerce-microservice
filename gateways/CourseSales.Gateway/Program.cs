var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureAppConfiguration((hostingContext, configure) =>
{
    

    
    configure.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
             .AddJsonFile("appsettings.json", true, true)
             .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
             .AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
             .AddJsonFile($"ocelot.global.json", true, true)
             .AddJsonFile($"ocelot.SwaggerEndpoint.json", true, true)
             .AddEnvironmentVariables();
});


builder.Services.AddControllers();
builder.Services.AddOcelot(builder.Configuration).AddDelegatingHandler<TokenExchangeDelegateHandler>();
builder.Services.AddSwaggerForOcelot(builder.Configuration);

builder.Services.AddHttpClient<TokenExchangeDelegateHandler>();



builder.Services.AddAuthentication().AddJwtBearer("GatewayAuthenticationSchema", options =>
{
    options.Authority = builder.Configuration["IdentityServerUrl"];
    options.Audience = "resource_gateway";
    options.RequireHttpsMetadata = false;
});var app = builder.Build();

app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
});

await app.UseOcelot();

app.Run();
