using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiGateway" });

    c.AddSecurityDefinition("Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter into field the word 'Bearer' following by space and JWT",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.Configure<DatabaseSetting>(builder.Configuration.GetRequiredSection(nameof(DatabaseSetting)));
builder.Services.AddSingleton<IDatabaseSetting>(provider => provider.GetRequiredService<IOptions<DatabaseSetting>>().Value);

builder.Services.AddScoped<IMongoContext, MongoContext>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<ICourseService, CourseManager>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["IdentityServerUrl"];
        options.Audience = "resource_catalog";
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddMassTransit(serviceCollectionBusConfigurator =>
{
    serviceCollectionBusConfigurator.UsingRabbitMq((busRegistrationContext, rabbitMqBusFactoryConfigurator) =>
    {
        string rabbitMQUrl = builder.Configuration["RabbitMQUrl"]; //Default port: 5672
        rabbitMqBusFactoryConfigurator.Host(rabbitMQUrl, "/", hostConfigurator =>
        {
            hostConfigurator.Username("guest");
            hostConfigurator.Password("guest");
        });
    });
});

var app = builder.Build();

await using (AsyncServiceScope asyncServiceScope = app.Services.CreateAsyncScope())
{
    IServiceProvider serviceProvider = asyncServiceScope.ServiceProvider;
    var categoryService = serviceProvider.GetRequiredService<ICategoryService>();
    var categories = await categoryService.GetAllAsync();
    if (categories.Data is null || !categories.Data.Any())
    {
        await categoryService.InsertAsync(new AddCategoryRequestModel
        {
            Name = "S�f�rdan �leri Seviye Vue.JS E�itimi ve Uygulama Geli�tirme"
        });

        await categoryService.InsertAsync(new AddCategoryRequestModel
        {
            Name = "Azure DevOps : S�f�rdan �leri Seviye"
        });
    }
};

app.UseAuthentication();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();