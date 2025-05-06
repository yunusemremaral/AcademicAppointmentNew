using AcademicAppointmentShare.Dtos.RoomDtos;
using AcademicAppointmentShare.Dtos.UserDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace AcademicAppointmentAdminMvc.MvcProject.Controllers
{
    public class AdminMvcRoomController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminMvcRoomController(IHttpClientFactory httpClientFactory)
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

        // GET: AdminMvcRoom
        public async Task<IActionResult> Index()
        {
            var client = CreateClient();
            var response = await client.GetAsync("api/admin/AdminRoom");

            if (!response.IsSuccessStatusCode)
                return View(new List<RoomDto>());

            var jsonData = await response.Content.ReadAsStringAsync();
            var rooms = JsonConvert.DeserializeObject<List<RoomDto>>(jsonData);
            return View(rooms);
        }

        // GET: AdminMvcRoom/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            var client = CreateClient();

            var response = await client.GetAsync($"api/admin/AdminRoom/details/{id}");
            if (!response.IsSuccessStatusCode) return NotFound();

            var room = await response.Content.ReadFromJsonAsync<RoomDetailDto>();
            return View(room);
        }

        // GET: AdminMvcRoom/Create
        public async Task<IActionResult> Create()
        {
            var client = CreateClient();
            var response = await client.GetAsync("api/admin/AdminUser/without-room");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Users = new List<SelectListItem>();
                return View();
            }

            var json = await response.Content.ReadAsStringAsync();
            var users = JsonConvert.DeserializeObject<List<UserSimpleDto>>(json);
         
            ViewBag.Users = users.Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = $"{u.UserFullName} ({u.Email})"
            }).ToList();

            return View();
        }

        // POST: AdminMvcRoom/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoomDto dto)
        {
            if (ModelState.IsValid)
            {
                var client = CreateClient();
                var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("api/admin/AdminRoom", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Oda eklerken bir hata oluştu.");
            }
            return View(dto);
        }

        // GET: AdminMvcRoom/Edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"api/admin/AdminRoom/{id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var jsonData = await response.Content.ReadAsStringAsync();
            var room = JsonConvert.DeserializeObject<RoomDto>(jsonData);
            var updateDto = new UpdateRoomDto
            {
                Id = room.Id,
                Name = room.Name,
                AppUserId = room.AppUserId
            };
            return View(updateDto);
        }

        // POST: AdminMvcRoom/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateRoomDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID uyuşmuyor.");

            if (ModelState.IsValid)
            {
                var client = CreateClient();
                var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = await client.PutAsync($"api/admin/AdminRoom", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Oda güncellenirken bir hata oluştu.");
            }
            return View(dto);
        }

        // POST: AdminMvcRoom/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"api/admin/AdminRoom/{id}");

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            TempData["Error"] = "Silme işlemi sırasında bir hata oluştu.";
            return RedirectToAction(nameof(Index));
        }
    }

}
