using AcademicAppointmentAdminMvc.MvcProject.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AcademicAppointmentAdminMvc.MvcProject.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AuthController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        // Login View
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Login Post - API'den JWT alıp cookie'ye kaydediyoruz
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // API'ye login isteği gönderiyoruz
            var client = _httpClientFactory.CreateClient("MyApi");
            var response = await client.PostAsJsonAsync("api/Auth/login", new { email = model.Email, password = model.Password });

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Giriş başarısız.");
                return View(model);
            }

            // Token'ı al
            var loginRes = await response.Content.ReadFromJsonAsync<LoginResponse>(); // LoginResponse modelinde Token var
            var token = loginRes?.Token;

            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError("", "Token alınamadı.");
                return View(model);
            }

            if (!TokenHasAdminRole(token))
            {
                return RedirectToAction("AccessDenied", "Auth");
            }



            // Token'ı cookie'ye kaydet
            Response.Cookies.Append("JwtToken", token, new CookieOptions
            {
                HttpOnly = true, // JavaScript erişemez
                Secure = true,   // HTTPS ile çalışır
                Expires = DateTimeOffset.UtcNow.AddMinutes(60) // 60 dk geçerli
            });

            // Kullanıcı claim'lerinden ClaimsPrincipal oluştur
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var identity = new ClaimsIdentity(jwtToken.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(identity);

            // Cookie-based oturum başlat
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                userPrincipal,
                new AuthenticationProperties { IsPersistent = true });

            return RedirectToAction("Dashboard", "Admin");
        }

        // Admin rolünü kontrol eden metod
        public bool TokenHasAdminRole(string token)
        {
            var claims = GetClaimsFromToken(token);
            return claims.Contains("Admin");
        }

        // Token'dan role'leri almak için
        private List<string> GetClaimsFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims
                .Where(c => c.Type == ClaimTypes.Role || c.Type == "role") // hem kısa hem uzun olanı kontrol et
                .Select(c => c.Value)
                .ToList();
        }

        // Logout işlemi
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Cookie'yi temizle
            Response.Cookies.Delete("JwtToken");

            // Oturumdan çık
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();

        }

    }
}
