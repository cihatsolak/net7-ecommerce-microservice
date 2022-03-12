var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

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
        options.Audience = "resource_discount";
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();
builder.Services.AddScoped<IDbConnection>(provider =>
{
    NpgsqlConnection npgsqlConnection = new(builder.Configuration.GetConnectionString("NpgSql"));
    return npgsqlConnection;
});

builder.Services.AddScoped<IDiscountService, DiscountService>();

var app = builder.Build();
app.UseAuthentication();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();
app.MapControllers();

app.Run();
