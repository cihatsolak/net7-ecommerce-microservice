var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RedisSetting>(builder.Configuration.GetRequiredSection(nameof(RedisSetting)));
builder.Services.AddSingleton<IRedisSetting>(provider => provider.GetRequiredService<IOptions<RedisSetting>>().Value);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();