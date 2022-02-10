var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.Configure<DatabaseSetting>(builder.Configuration.GetRequiredSection(nameof(DatabaseSetting)));
builder.Services.AddSingleton<IDatabaseSetting>(provider => provider.GetRequiredService<IOptions<DatabaseSetting>>().Value);

builder.Services.AddScoped<IMongoContext, MongoContext>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
app.Run();