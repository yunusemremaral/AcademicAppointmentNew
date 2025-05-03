using AcademicAppointmentShare.Dtos.NotificationDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AcademicAppointmentAdminMvc.MvcProject.Controllers
{
    public class AdminMvcNotificationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminMvcNotificationController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient("MyApi");
            var token = Request.Cookies["JwtToken"];
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

        public async Task<IActionResult> Index()
        {
            var client = CreateClient();
            var response = await client.GetAsync("api/admin/AdminNotification");

            if (!response.IsSuccessStatusCode)
                return View(new List<NotificationDto>());

            var json = await response.Content.ReadAsStringAsync();
            var notifications = JsonConvert.DeserializeObject<List<NotificationDto>>(json);
            return View(notifications);
        }

        public async Task<IActionResult> Details(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"api/admin/AdminNotification/{id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var notification = JsonConvert.DeserializeObject<NotificationDto>(await response.Content.ReadAsStringAsync());
            return View(notification);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateNotificationDto dto)
        {
            if (ModelState.IsValid)
            {
                var client = CreateClient();
                var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/admin/AdminNotification", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Bildirimi eklerken bir hata oluştu.");
            }

            return View(dto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"api/admin/AdminNotification/{id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var notification = JsonConvert.DeserializeObject<NotificationDto>(await response.Content.ReadAsStringAsync());
            var updateDto = new UpdateNotificationDto
            {
                Id = notification.Id,
                Title = notification.Title,
                Message = notification.Message,
                IsRead = notification.IsRead
            };

            return View(updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateNotificationDto dto)
        {
            if (ModelState.IsValid)
            {
                var client = CreateClient();
                var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"api/admin/AdminNotification/{dto.Id}", content);
                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Güncelleme başarısız.");
            }

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"api/admin/AdminNotification/{id}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            TempData["Error"] = "Silme işlemi başarısız.";
            return RedirectToAction(nameof(Index));
        }
    }
}
