using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using AcademicAppointmentAdminMvc.MvcProject.Models;
using AcademicAppointmentAdminMvc.MvcProject.Dtos;

namespace AcademicAppointmentAdminMvc.MvcProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUserMvcController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminUserMvcController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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
            var response = await client.GetAsync("https://localhost:7214/api/AdminUser/all");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var users = JsonConvert.DeserializeObject<List<UserDto>>(json);
                return View(users);
            }
            else
            {
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

            ModelState.AddModelError("", "Kullanıcı oluşturulamadı.");
            return View(model);
        }

        // Kullanıcı silme
        public async Task<IActionResult> Delete(string id)
        {
            var client = CreateClientWithToken();
            var response = await client.DeleteAsync($"https://localhost:7214/api/AdminUser/delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction(nameof(Index));
            }

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
                var user = JsonConvert.DeserializeObject<UserDto>(json);
                return View(user);
            }

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
                var user = JsonConvert.DeserializeObject<UserUpdateDto>(json);
                return View(user);
            }

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

            ModelState.AddModelError("", "Kullanıcı güncellenemedi.");
            return View(model);
        }
    }
}
