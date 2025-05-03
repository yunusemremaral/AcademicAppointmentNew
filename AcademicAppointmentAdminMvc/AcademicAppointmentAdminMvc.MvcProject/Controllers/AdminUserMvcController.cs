//using AcademicAppointmentAdminMvc.MvcProject.Dtos.UserMvcDtos;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Net.Http.Headers;
//using System.Text.Json;

//namespace AcademicAppointmentAdminMvc.MvcProject.Controllers
//{
//    [Authorize(Roles = "Admin")]
//    public class AdminUserMvcController : Controller
//    {
//        private readonly IHttpClientFactory _httpClientFactory;
//        private readonly IConfiguration _config;
//        private readonly ILogger<AdminUserMvcController> _logger;
//        private readonly string _apiBaseUrl;

//        public AdminUserMvcController(
//            IHttpClientFactory httpClientFactory,
//            IConfiguration config,
//            ILogger<AdminUserMvcController> logger)
//        {
//            _httpClientFactory = httpClientFactory;
//            _config = config;
//            _logger = logger;
//            _apiBaseUrl = _config["ApiBaseUrl"]?.TrimEnd('/'); // URL'deki son '/' karakterini kaldır
//        }

//        // HttpClient oluştur ve token'ı header'a ekle (JwtCookieHandler ile entegre)
//        private HttpClient CreateClient()
//        {
//            var client = _httpClientFactory.CreateClient("MyApi"); // Program.cs'de tanımlanan MyApi client'ını kullan
//            var token = Request.Cookies["JwtToken"];
//            if (!string.IsNullOrEmpty(token))
//            {
//                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
//            }
//            return client;
//        }

//        // Kullanıcı Listeleme (Rol Filtreli)
//        public async Task<IActionResult> Index(string roleFilter = null)
//        {
//            try
//            {
//                var client = CreateClient();
//                var url = $"{_apiBaseUrl}/api/admin/AdminUser";

//                // Rol filtresi ekle (boş değilse)
//                if (!string.IsNullOrWhiteSpace(roleFilter))
//                {
//                    url += $"?role={roleFilter}";
//                }

//                var response = await client.GetAsync(url);
//                response.EnsureSuccessStatusCode(); // 200-299 dışındaki kodlarda hata fırlatır

//                var jsonResponse = await response.Content.ReadAsStringAsync();
//                _logger.LogInformation("API Response: {jsonResponse}", jsonResponse);

//                var users = await response.Content.ReadFromJsonAsync<List<AdminUserListMvcDto>>();
//                return View(users);
//            }
//            catch (HttpRequestException ex)
//            {
//                _logger.LogError(ex, "API isteği başarısız. URL: {url}", $"{_apiBaseUrl}/api/admin/adminuser");
//                TempData["Error"] = "API ile iletişim kurulamadı!";
//                return View(new List<AdminUserListMvcDto>());
//            }
//            catch (JsonException ex)
//            {
//                _logger.LogError(ex, "JSON deserializasyon hatası!");
//                TempData["Error"] = "Veri okunamadı!";
//                return View(new List<AdminUserListMvcDto>());
//            }
//        }

//        // Kullanıcı Detayları
//        public async Task<IActionResult> Details(string id)
//        {
//            try
//            {
//                var client = CreateClient();
//                var response = await client.GetAsync($"{_apiBaseUrl}/api/admin/adminuser/{id}");
//                response.EnsureSuccessStatusCode();

//                var user = await response.Content.ReadFromJsonAsync<AdminUserDetailMvcDto>();
//                return View(user);
//            }
//            catch (HttpRequestException)
//            {
//                TempData["Error"] = "Kullanıcı bulunamadı!";
//                return RedirectToAction(nameof(Index));
//            }
//        }

//        // Rol Ata
//        [HttpPost]
//        public async Task<IActionResult> AssignRole(string userId, string role)
//        {
//            try
//            {
//                var client = CreateClient();
//                var response = await client.PostAsJsonAsync(
//                    $"{_apiBaseUrl}/api/admin/adminuser/{userId}/roles",
//                    new AssignRoleMvcDto { Role = role });

//                response.EnsureSuccessStatusCode();
//                TempData["Success"] = "Rol başarıyla atandı!";
//            }
//            catch (HttpRequestException)
//            {
//                TempData["Error"] = "Rol atama başarısız!";
//            }
//            return RedirectToAction(nameof(Details), new { id = userId });
//        }

//        // Rol Kaldır
//        [HttpPost]
//        public async Task<IActionResult> RemoveRole(string userId, string role)
//        {
//            try
//            {
//                var client = CreateClient();
//                var response = await client.DeleteAsync(
//                    $"{_apiBaseUrl}/api/admin/adminuser/{userId}/roles?role={role}");

//                response.EnsureSuccessStatusCode();
//                TempData["Success"] = "Rol başarıyla kaldırıldı!";
//            }
//            catch (HttpRequestException)
//            {
//                TempData["Error"] = "Rol kaldırma başarısız!";
//            }
//            return RedirectToAction(nameof(Details), new { id = userId });
//        }
//    }
//}