var builder = WebApplication.CreateBuilder(args);

builder.AddConfigures();
builder.AddHttpClientServices();

builder.Services.AddScoped<ResourceOwnerPasswordTokenHandler>();
builder.Services.AddScoped<ClientCredentialTokenHandler>();
builder.Services.AddSingleton<PhotoHelper>();
builder.Services.AddAccessTokenManagement();
builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CourseCreateInputValidator>());

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


app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
