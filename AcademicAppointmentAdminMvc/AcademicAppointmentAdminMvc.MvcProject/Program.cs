using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using AcademicAppointmentAdminMvc.MvcProject.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. MVC Servislerini ekle
builder.Services.AddControllersWithViews();

// 2. Cookie Authentication yapılandırması
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)  // Cookie Authentication kullanıyoruz
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";  // Giriş yapılmadıysa bu URL'ye yönlendirme yapılacak
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);  // Token ve oturum süresi
        options.SlidingExpiration = true;  // Oturum süresi sona ermeden önce aktivitelerle uzatılır
    });

// 3. IHttpContextAccessor Servisini DI'ya ekle
builder.Services.AddHttpContextAccessor();  // HTTP bağlamına erişmek için kullanılır

// 4. API çağrıları için HttpClient yapılandırması
builder.Services.AddHttpClient("MyApi", client =>
{
    var apiBaseUrl = builder.Configuration["ApiBaseUrl"];  // ApiBaseUrl'i configuration'dan alıyoruz
    if (string.IsNullOrEmpty(apiBaseUrl))
    {
        throw new ArgumentNullException("ApiBaseUrl", "API base URL is not configured properly.");  // Hata fırlatıyoruz
    }
    client.BaseAddress = new Uri(apiBaseUrl);  // API'nin ana adresini belirliyoruz
})
.AddHttpMessageHandler<JwtCookieHandler>();  // API isteklerine ek handler (JwtCookieHandler) ekliyoruz

// 5. JwtCookieHandler'ı DI'ya ekleyin
builder.Services.AddTransient<JwtCookieHandler>();  // Bu handler, cookie'deki JWT token'ı otomatik olarak işlemek için kullanılır

var app = builder.Build();

// 6. Middleware sırasını ayarla
app.UseHttpsRedirection();  // HTTP'den HTTPS'ye yönlendirme
app.UseStaticFiles();  // Statik dosyalar için middleware
app.UseRouting();  // Yönlendirme işlemleri
app.UseAuthentication();  // Kimlik doğrulama işlemi (cookie tabanlı)
app.UseAuthorization();  // Yetkilendirme işlemi (rol ve izinler)

// 7. Controller route yapılandırması
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");  // Varsayılan controller ve action

// 8. Uygulamayı başlat
app.Run();  // Uygulama çalışmaya başlar
