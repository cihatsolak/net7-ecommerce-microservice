var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrderDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlServerOptionsAction =>
    {
        sqlServerOptionsAction.MigrationsAssembly(typeof(OrderDbContext).Assembly.FullName);
    });
});

var authorizationPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter(authorizationPolicy));
});
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServerUrl"];
        options.Audience = "resource_order";
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddMassTransit(serviceCollectionBusConfigurator =>
{
    serviceCollectionBusConfigurator.AddConsumer<CreateOrderMessageCommandConsumer>();
    serviceCollectionBusConfigurator.AddConsumer<CourseNameChangedEventConsumer>();

    serviceCollectionBusConfigurator.UsingRabbitMq((busRegistrationContext, rabbitMqBusFactoryConfigurator) =>
    {
        string rabbitMQUrl = builder.Configuration["RabbitMQUrl"]; //Default port: 5672
        rabbitMqBusFactoryConfigurator.Host(rabbitMQUrl, "/", hostConfigurator =>
        {
            hostConfigurator.Username("guest");
            hostConfigurator.Password("guest");
        });

        rabbitMqBusFactoryConfigurator.ReceiveEndpoint("create-order-service", options =>
        {
            options.ConfigureConsumer<CreateOrderMessageCommandConsumer>(busRegistrationContext);
        });

        rabbitMqBusFactoryConfigurator.ReceiveEndpoint("course-name-changed-event-order-service", options =>
        {
            options.ConfigureConsumer<CourseNameChangedEventConsumer>(busRegistrationContext);
        });
    });
});

builder.Services.AddMediatR(typeof(CreateOrderCommandHandler).Assembly);
builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

await using (AsyncServiceScope asyncServiceScope = app.Services.CreateAsyncScope())
{
    IServiceProvider serviceProvider = asyncServiceScope.ServiceProvider;
    OrderDbContext orderDbContext = serviceProvider.GetRequiredService<OrderDbContext>();
    await orderDbContext.Database.MigrateAsync();
};

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
