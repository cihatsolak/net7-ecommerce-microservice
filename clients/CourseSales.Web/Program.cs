var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ServiceApiSettings>(builder.Configuration.GetRequiredSection(nameof(ServiceApiSettings)));
var serviceApiSettings = builder.Configuration.GetSection(nameof(ServiceApiSettings)).Get<ServiceApiSettings>();

builder.Services.Configure<ClientSettings>(builder.Configuration.GetRequiredSection(nameof(ClientSettings)));

builder.Services.AddHttpClient<IClientCredentialTokenService, ClientCredentialTokenService>();
builder.Services.AddHttpClient<IIdentityService, IdentityService>();

builder.Services.AddScoped<ResourceOwnerPasswordTokenHandler>();
builder.Services.AddScoped<ClientCredentialTokenHandler>();

builder.Services.AddHttpClient<IUserService, UserService>(options =>
{
    options.BaseAddress = new Uri(serviceApiSettings.IdentityBaseUri);
}).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

builder.Services.AddHttpClient<ICatalogService, CatalogService>(options =>
{
    options.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}{serviceApiSettings.Catalog.Path}");
}).AddHttpMessageHandler<ClientCredentialTokenHandler>();

builder.Services.AddHttpClient<IPhotoStockService, PhotoStockService>(options =>
{
    options.BaseAddress = new Uri($"{serviceApiSettings.GatewayBaseUri}{serviceApiSettings.PhotoStock.Path}");
}).AddHttpMessageHandler<ClientCredentialTokenHandler>();


builder.Services.AddAccessTokenManagement();
builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, configure =>
    {
        configure.LoginPath = new PathString("/Auth/SignIn");
        configure.ExpireTimeSpan = TimeSpan.FromDays(60);
        configure.SlidingExpiration = true;
        configure.Cookie.Name = "CourseSales.Web";
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
