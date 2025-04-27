using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using AcademicAppointmentAdminMvc.MvcProject.Models;

var builder = WebApplication.CreateBuilder(args);

// MVC ve Cookie-Auth yapılandırması
builder.Services.AddControllersWithViews();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
        options.SlidingExpiration = true;
    });

// IHttpContextAccessor'ı DI'ya ekleyin
builder.Services.AddHttpContextAccessor();

// API çağrıları için HttpClient ve handler
builder.Services.AddHttpClient("MyApi", client =>
{
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"];  // ApiBaseUrl'i buradan alıyoruz
    if (string.IsNullOrEmpty(apiBaseUrl))
    {
        throw new ArgumentNullException("ApiBaseUrl", "API base URL is not configured properly.");
    }
    client.BaseAddress = new Uri(apiBaseUrl);  // ApiBaseUrl'i kullanıyoruz
})
.AddHttpMessageHandler<JwtCookieHandler>();

// JWT Cookie Handler'ı DI'ya ekleyin
builder.Services.AddTransient<JwtCookieHandler>();

var app = builder.Build();

// Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
