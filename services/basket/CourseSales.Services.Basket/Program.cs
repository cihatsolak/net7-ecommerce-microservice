using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

var authorizationPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter(authorizationPolicy));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServerUrl"];
        options.Audience = "resource_basket";
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddMassTransit(serviceCollectionBusConfigurator =>
{
    serviceCollectionBusConfigurator.AddConsumer<CourseNameChangedEventConsumer>();

    serviceCollectionBusConfigurator.UsingRabbitMq((busRegistrationContext, rabbitMqBusFactoryConfigurator) =>
    {
        string rabbitMQUrl = builder.Configuration["RabbitMQUrl"]; //Default port: 5672
        rabbitMqBusFactoryConfigurator.Host(rabbitMQUrl, "/", hostConfigurator =>
        {
            hostConfigurator.Username("guest");
            hostConfigurator.Password("guest");
        });

        rabbitMqBusFactoryConfigurator.ReceiveEndpoint("course-name-changed-event-basket-service", options =>
        {
            options.ConfigureConsumer<CourseNameChangedEventConsumer>(busRegistrationContext);
        });
    });
});


builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();
builder.Services.AddScoped<IBasketService, BasketService>();

RedisSetting redisSetting = builder.Configuration.GetSection(nameof(RedisSetting)).Get<RedisSetting>();
builder.Services.AddSingleton<IRedisService, RedisService>();
builder.Services.AddSingleton<IConnectionMultiplexer>(provider => ConnectionMultiplexer.Connect(new ConfigurationOptions
{
    EndPoints = { redisSetting.ConnectionString }
}));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();