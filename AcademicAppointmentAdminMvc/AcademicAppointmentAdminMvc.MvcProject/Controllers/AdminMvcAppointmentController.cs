using AcademicAppointmentShare.Dtos.AppointmentDtoS;
using AcademicAppointmentShare.Dtos.UserDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AcademicAppointmentAdminMvc.MvcProject.Controllers
{
    public class AdminMvcAppointmentController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminMvcAppointmentController(IHttpClientFactory httpClientFactory)
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

        private async Task<List<UserDto>> GetUsersByRole(string role)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"/api/admin/AdminUser/by-role/{role}");
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<List<UserDto>>()
                : new List<UserDto>();
        }

        // GET: AdminMvcAppointment
        public async Task<IActionResult> Index(string? status, DateTime? startDate, DateTime? endDate)
        {
            var client = CreateClient();
            List<AppointmentResultDto> appointments;

            if (startDate.HasValue && endDate.HasValue)
            {
                var response = await client.GetAsync($"/api/admin/AdminAppointment/range?start={startDate:o}&end={endDate:o}");
                appointments = response.IsSuccessStatusCode
                    ? await response.Content.ReadFromJsonAsync<List<AppointmentResultDto>>()
                    : new List<AppointmentResultDto>();
            }
            else if (!string.IsNullOrEmpty(status))
            {
                var statusEnum = Enum.Parse<AppointmentStatusDto>(status);
                var response = await client.GetAsync($"/api/admin/AdminAppointment/status/{(int)statusEnum}");
                appointments = response.IsSuccessStatusCode
                    ? await response.Content.ReadFromJsonAsync<List<AppointmentResultDto>>()
                    : new List<AppointmentResultDto>();
            }
            else
            {
                var response = await client.GetAsync("/api/admin/AdminAppointment/all");
                appointments = response.IsSuccessStatusCode
                    ? await response.Content.ReadFromJsonAsync<List<AppointmentResultDto>>()
                    : new List<AppointmentResultDto>();
            }

            ViewBag.StatusList = new SelectList(Enum.GetNames(typeof(AppointmentStatusDto)));
            return View(appointments);
        }

        // GET: AdminMvcAppointment/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"/api/admin/AdminAppointment/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Randevu bulunamadı!";
                return RedirectToAction(nameof(Index));
            }

            var appointment = await response.Content.ReadFromJsonAsync<AppointmentResultDto>();
            return View(appointment);
        }

        // GET: AdminMvcAppointment/Create
        public async Task<IActionResult> Create()
        {
            var client = CreateClient();

            var academics = await GetUsersByRole("Instructor");
            var students = await GetUsersByRole("Student");

            ViewBag.AcademicList = new SelectList(academics, "Id", "Email");
            ViewBag.StudentList = new SelectList(students, "Id", "Email");

            return View();
        }

        // POST: AdminMvcAppointment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppointmentCreateDto dto)
        {
            var client = CreateClient();
            var response = await client.PostAsJsonAsync("/api/admin/AdminAppointment", dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Randevu başarıyla oluşturuldu!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Randevu oluşturulamadı!";
            var academics = await GetUsersByRole("Instructor");
            var students = await GetUsersByRole("Student");

            ViewBag.AcademicList = new SelectList(academics, "Id", "Email", dto.AcademicUserId);
            ViewBag.StudentList = new SelectList(students, "Id", "Email", dto.StudentUserId);

            return View(dto);
        }

        // GET: AdminMvcAppointment/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"/api/admin/AdminAppointment/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Randevu bulunamadı!";
                return RedirectToAction(nameof(Index));
            }

            var appointment = await response.Content.ReadFromJsonAsync<AppointmentResultDto>();
            var updateDto = new AppointmentUpdateDto
            {
                Id = appointment.Id,
                ScheduledAt = appointment.ScheduledAt,
                Subject = appointment.Subject,
                Description = appointment.Description,
                Status = (int)appointment.Status
            };

            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(AppointmentStatusDto))
                .Cast<AppointmentStatusDto>()
                .Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }), "Value", "Text", updateDto.Status);

            return View(updateDto);
        }

        // POST: AdminMvcAppointment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AppointmentUpdateDto dto)
        {
            var client = CreateClient();
            var response = await client.PutAsJsonAsync("/api/admin/AdminAppointment", dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Randevu başarıyla güncellendi!";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Randevu güncellenemedi!";
            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(AppointmentStatusDto))
                .Cast<AppointmentStatusDto>()
                .Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }), "Value", "Text", dto.Status);

            return View(dto);
        }

        // POST: AdminMvcAppointment/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"/api/admin/AdminAppointment/{id}");

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Randevu başarıyla silindi!";
            else
                TempData["Error"] = "Randevu silinemedi!";

            return RedirectToAction(nameof(Index));
        }

        // GET: AdminMvcAppointment/ChangeStatus/5
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"/api/admin/AdminAppointment/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Randevu bulunamadı!";
                return RedirectToAction(nameof(Index));
            }

            var appointment = await response.Content.ReadFromJsonAsync<AppointmentResultDto>();
            var statusDto = new AppointmentStatusUpdateDto
            {
                AppointmentId = id,
                Status = (int)appointment.Status
            };

            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(AppointmentStatusDto))
                .Cast<AppointmentStatusDto>()
                .Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }), "Value", "Text", statusDto.Status);

            return View(statusDto);
        }

        // POST: AdminMvcAppointment/ChangeStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(AppointmentStatusUpdateDto dto)
        {
            var client = CreateClient();
            var response = await client.PutAsJsonAsync("/api/admin/AdminAppointment/update-status", dto);

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Durum başarıyla güncellendi!";
            else
                TempData["Error"] = "Durum güncellenemedi!";

            return RedirectToAction(nameof(Index));
        }
    }
}