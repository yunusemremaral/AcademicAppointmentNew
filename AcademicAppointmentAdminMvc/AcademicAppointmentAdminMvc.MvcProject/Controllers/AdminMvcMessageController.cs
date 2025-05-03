using AcademicAppointmentShare.Dtos.MessageDtos;
using AcademicAppointmentShare.Dtos.UserDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AcademicAppointmentAdminMvc.MvcProject.Controllers
{
    public class AdminMvcMessageController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminMvcMessageController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private HttpClient CreateClient()
        {
            var client = _httpClientFactory.CreateClient("MyApi");
            var token = Request.Cookies["JwtToken"];
            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }

        private async Task<List<UserDto>> GetUsersAsync(HttpClient client)
        {
            var response = await client.GetAsync("/api/admin/AdminUser");
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<List<UserDto>>()
                : new List<UserDto>();
        }

        // GET: AdminMvcMessage
        public async Task<IActionResult> Index(string filterEmail)
        {
            var client = CreateClient();
            List<ResultMessageDto> messages;

            // Kullanıcı filtreleme için SelectList
            var users = await GetUsersAsync(client);
            ViewBag.UserEmails = new SelectList(users.Select(u => u.Email).Distinct());

            if (!string.IsNullOrEmpty(filterEmail))
            {
                var user = users.FirstOrDefault(u => u.Email == filterEmail);
                if (user == null)
                {
                    TempData["Error"] = "Kullanıcı bulunamadı!";
                    return View(new List<ResultMessageDto>());
                }

                var response = await client.GetAsync($"/api/admin/AdminMessage/user/{user.Id}");
                messages = response.IsSuccessStatusCode
                    ? await response.Content.ReadFromJsonAsync<List<ResultMessageDto>>()
                    : new List<ResultMessageDto>();
            }
            else
            {
                var response = await client.GetAsync("/api/admin/AdminMessage/with-relations");
                messages = response.IsSuccessStatusCode
                    ? await response.Content.ReadFromJsonAsync<List<ResultMessageDto>>()
                    : new List<ResultMessageDto>();
            }

            return View(messages);
        }

        // GET: AdminMvcMessage/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"/api/admin/AdminMessage/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Mesaj bulunamadı!";
                return RedirectToAction(nameof(Index));
            }

            var message = await response.Content.ReadFromJsonAsync<ResultMessageDto>();
            return View(message);
        }

        // GET: AdminMvcMessage/Create
        public async Task<IActionResult> Create()
        {
            var client = CreateClient();
            var users = await GetUsersAsync(client);

            ViewBag.SenderList = new SelectList(users, "Id", "Email");
            ViewBag.ReceiverList = new SelectList(users, "Id", "Email");

            return View();
        }

        // POST: AdminMvcMessage/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateMessageDto dto)
        {
            var client = CreateClient();
            var response = await client.PostAsJsonAsync("/api/admin/AdminMessage", dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Mesaj başarıyla gönderildi!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Mesaj gönderilemedi!";
            var users = await GetUsersAsync(client);
            ViewBag.SenderList = new SelectList(users, "Id", "Email", dto.SenderId);
            ViewBag.ReceiverList = new SelectList(users, "Id", "Email", dto.ReceiverId);
            return View(dto);
        }

        // GET: AdminMvcMessage/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"/api/admin/AdminMessage/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Mesaj bulunamadı!";
                return RedirectToAction(nameof(Index));
            }

            var message = await response.Content.ReadFromJsonAsync<ResultMessageDto>();
            var updateDto = new UpdateMessageDto { Id = message.Id, Content = message.Content };

            return View(updateDto);
        }

        // POST: AdminMvcMessage/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateMessageDto dto)
        {
            var client = CreateClient();
            var response = await client.PutAsJsonAsync("/api/admin/AdminMessage", dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Mesaj başarıyla güncellendi!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Mesaj güncellenemedi!";
            return View(dto);
        }

        // GET: AdminMvcMessage/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"/api/admin/AdminMessage/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Mesaj bulunamadı!";
                return RedirectToAction(nameof(Index));
            }

            var message = await response.Content.ReadFromJsonAsync<ResultMessageDto>();
            return View(message);
        }

        // POST: AdminMvcMessage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"/api/admin/AdminMessage/{id}");

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Mesaj başarıyla silindi!";
            else
                TempData["Error"] = "Mesaj silinemedi!";

            return RedirectToAction(nameof(Index));
        }

        // GET: AdminMvcMessage/Conversation
        public async Task<IActionResult> Conversation(string userId1, string userId2)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"/api/admin/AdminMessage/conversation?userId1={userId1}&userId2={userId2}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Sohbet getirilemedi!";
                return RedirectToAction(nameof(Index));
            }

            var messages = await response.Content.ReadFromJsonAsync<List<ResultMessageDto>>();

            // View'da kimin gönderdiğini anlamak için
            ViewBag.UserId1 = userId1;
            ViewBag.UserId2 = userId2;

            return View(messages);
        }
    }
}