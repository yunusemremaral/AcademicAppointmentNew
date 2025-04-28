using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using AcademicAppointmentAdminMvc.MvcProject.Dtos.UserMvcDtos;

namespace AcademicAppointmentAdminMvc.MvcProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUserMvcController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AdminUserMvcController> _logger;

        public AdminUserMvcController(IHttpClientFactory httpClientFactory, ILogger<AdminUserMvcController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        private HttpClient CreateClientWithToken()
        {
            var client = _httpClientFactory.CreateClient();
            var token = Request.Cookies["JwtToken"];
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        // Tüm kullanıcıları listele
        public async Task<IActionResult> Index()
        {
            var client = CreateClientWithToken(); 

            var response = await client.GetAsync("https://localhost:7214/api/AdminUser");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var users = JsonSerializer.Deserialize<List<UserDto>>(json);
                return View(users);
            }
            else
            {
                _logger.LogError("Failed to load users. Status code: {StatusCode}", response.StatusCode);
                ModelState.AddModelError("", "Kullanıcılar alınamadı.");
                return View(new List<UserDto>());
            }
        }

        // Yeni kullanıcı ekleme GET
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Yeni kullanıcı ekleme POST
        [HttpPost]
        public async Task<IActionResult> Create(UserCreateDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = CreateClientWithToken();
            var response = await client.PostAsJsonAsync("https://localhost:7214/api/AdminUser/create", model);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            _logger.LogError("Failed to create user. Status code: {StatusCode}", response.StatusCode);
            ModelState.AddModelError("", "Kullanıcı oluşturulamadı.");
            return View(model);
        }

        // Kullanıcı silme
        public async Task<IActionResult> Delete(string id)
        {
            var client = CreateClientWithToken();
            var response = await client.DeleteAsync($"https://localhost:7214/api/AdminUser/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            _logger.LogError("Failed to delete user with ID {UserId}. Status code: {StatusCode}", id, response.StatusCode);
            ModelState.AddModelError("", "Kullanıcı silinemedi.");
            return RedirectToAction(nameof(Index));
        }

        // Kullanıcı detaylarını gösterme
        public async Task<IActionResult> Details(string id)
        {
            var client = CreateClientWithToken();
            var response = await client.GetAsync($"https://localhost:7214/api/AdminUser/{id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<UserDto>(json);
                return View(user);
            }

            _logger.LogError("Failed to fetch user details for ID {UserId}. Status code: {StatusCode}", id, response.StatusCode);
            ModelState.AddModelError("", "Kullanıcı bilgileri alınamadı.");
            return RedirectToAction(nameof(Index));
        }

        // Kullanıcı güncelleme GET
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var client = CreateClientWithToken();
            var response = await client.GetAsync($"https://localhost:7214/api/AdminUser/{id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<UserUpdateDto>(json);
                return View(user);
            }

            _logger.LogError("Failed to fetch user details for editing, User ID: {UserId}", id);
            ModelState.AddModelError("", "Kullanıcı bilgileri alınamadı.");
            return RedirectToAction(nameof(Index));
        }

        // Kullanıcı güncelleme POST
        [HttpPost]
        public async Task<IActionResult> Edit(UserUpdateDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var client = CreateClientWithToken();
            var response = await client.PutAsJsonAsync($"https://localhost:7214/api/AdminUser/update/{model.Id}", model);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

            _logger.LogError("Failed to update user. Status code: {StatusCode}", response.StatusCode);
            ModelState.AddModelError("", "Kullanıcı güncellenemedi.");
            return View(model);
        }
    }
}
