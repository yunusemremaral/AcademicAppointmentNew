using AcademicAppointmentShare.Dtos.UserDtos;
using AcademicAppointmentShare.Dtos.RoleDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using AcademicAppointmentShare.Dtos.RoomDtos;
using Microsoft.AspNetCore.Authorization;
using AcademicAppointmentAdminMvc.MvcProject.Models;
using AcademicAppointmentShare.Dtos.DepartmentDtos;
using AcademicAppointmentShare.Dtos.SchoolDtos;

namespace AcademicAppointmentAdminMvc.MvcProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminMvcUserController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminMvcUserController(IHttpClientFactory httpClientFactory)
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

        // GET: AdminUserMvc
        public async Task<IActionResult> Index(string roleFilter, string searchQuery)
        {
            var client = CreateClient();
            var response = await client.GetAsync("/api/admin/AdminUser/users");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Kullanıcılar getirilemedi!";
                return View(new List<UserDto>());
            }

            var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();

            // Filtreleme
            if (!string.IsNullOrEmpty(roleFilter))
                users = await FilterByRole(users, roleFilter);

            if (!string.IsNullOrEmpty(searchQuery))
                users = users.Where(u =>
                    u.Email.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    u.UserName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)
                ).ToList();

            ViewBag.Roles = await GetRoleList();
            return View(users);
        }

        // GET: AdminUserMvc/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var client = CreateClient(); 

            var response = await client.GetAsync($"/api/admin/AdminUser/{id}/details");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Kullanıcı bulunamadı!";
                return RedirectToAction(nameof(Index));
            }

            var user = await response.Content.ReadFromJsonAsync<UserWithDetailsDto>();
            return View(user);
        }

        // GET: AdminUserMvc/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View(new CreateUserDto()); // Okul oluşturma DTO'su değil, kullanıcı oluşturma DTO'su
        }

        // POST: AdminUserMvc/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(dto.SchoolId);
                return View(dto);
            }

            var client = CreateClient();
            var response = await client.PostAsJsonAsync("/api/admin/AdminUser", dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Kullanıcı başarıyla oluşturuldu!";
                return RedirectToAction(nameof(Index));
            }

            var error = await response.Content.ReadAsStringAsync();
            TempData["Error"] = $"Hata: {error}";
            await PopulateDropdowns(dto.SchoolId);
            return View(dto);
        }

        // GET: AdminUserMvc/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"/api/admin/AdminUser/{id}/details"); // Detay endpointini kullan

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Kullanıcı bulunamadı!";
                return RedirectToAction(nameof(Index));
            }

            var user = await response.Content.ReadFromJsonAsync<UserWithDetailsDto>();
            await PopulateDropdowns();

            // Update DTO'suna çevir
            var updateDto = new UpdateUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                SchoolId = user.SchoolId,
                DepartmentId = user.DepartmentId,
                RoomId = user.RoomId
            };

            return View(updateDto);
        }

        // POST: AdminUserMvc/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UpdateUserDto dto)
        {
            if (id != dto.Id)
            {
                TempData["Error"] = "Geçersiz kullanıcı ID!";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                var client = CreateClient();
                var response = await client.PutAsJsonAsync($"/api/admin/AdminUser/{id}", dto);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] = "Kullanıcı başarıyla güncellendi!";
                    return RedirectToAction(nameof(Index));
                }

                var error = await response.Content.ReadAsStringAsync();
                TempData["Error"] = $"Güncelleme hatası: {error}";
            }

            await PopulateDropdowns();
            return View(dto);
        }

        // POST: AdminUserMvc/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"/api/admin/AdminUser/{id}");

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Kullanıcı başarıyla silindi!";
            else
                TempData["Error"] = "Silme işlemi başarısız!";

            return RedirectToAction(nameof(Index));
        }

        // GET: AdminUserMvc/ManageRoles/5
        public async Task<IActionResult> ManageRoles(string id)
        {
            var client = CreateClient();

            var userResponse = await client.GetAsync($"/api/admin/AdminUser/{id}");
            if (!userResponse.IsSuccessStatusCode)
            {
                TempData["Error"] = "Kullanıcı bulunamadı!";
                return RedirectToAction(nameof(Index));
            }

            var rolesResponse = await client.GetAsync("/api/admin/AdminRole");
            if (!rolesResponse.IsSuccessStatusCode)
            {
                TempData["Error"] = "Roller getirilemedi!";
                return RedirectToAction(nameof(Index));
            }

            var user = await userResponse.Content.ReadFromJsonAsync<UserDto>();
            var roles = await rolesResponse.Content.ReadFromJsonAsync<List<RoleDto>>();
            var userRoles = await client.GetFromJsonAsync<List<string>>($"/api/admin/AdminUser/{id}/roles");

            var model = new UserRoleManagementDto
            {
                UserId = id,
                UserName = user.UserName,
                AllRoles = roles.Select(r => new SelectListItem(r.Name, r.Name)).ToList(),
                AssignedRoles = userRoles
            };

            return View(model);
        }

        // POST: AdminUserMvc/AssignRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(RoleAssignmentDto dto)
        {
            var client = CreateClient();
            var response = await client.PostAsJsonAsync("/api/admin/AdminRole/assign-role", dto);

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Rol başarıyla atandı!";
            else
                TempData["Error"] = "Rol atama başarısız!";

            return RedirectToAction("ManageRoles", new { id = dto.UserId });
        }

        // POST: AdminUserMvc/RemoveRole
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRole(string userId, string roleName)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"/api/admin/AdminUser/{userId}/roles/{roleName}");

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Rol başarıyla kaldırıldı!";
            else
                TempData["Error"] = "Rol kaldırma başarısız!";

            return RedirectToAction("ManageRoles", new { id = userId });
        }

        #region Helper Methods
        private async Task<List<UserDto>> FilterByRole(List<UserDto> users, string roleName)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"/api/admin/AdminUser/by-role/{roleName}");
            if (!response.IsSuccessStatusCode) return users;

            var usersInRole = await response.Content.ReadFromJsonAsync<List<UserDto>>();
            return users.Where(u => usersInRole.Any(ur => ur.Id == u.Id)).ToList();
        }

        private async Task<List<SelectListItem>> GetRoleList()
        {
            var client = CreateClient();
            var response = await client.GetAsync("/api/admin/AdminRole");
            if (!response.IsSuccessStatusCode) return new List<SelectListItem>();

            var roles = await response.Content.ReadFromJsonAsync<List<RoleDto>>();
            return roles.Select(r => new SelectListItem(r.Name, r.Name)).ToList();
        }

        private async Task PopulateDropdowns(int? schoolId = null)
        {
            var client = CreateClient();

            // Okulları çek
            var schools = await client.GetFromJsonAsync<List<SchoolListDto>>("api/admin/AdminSchool");
            ViewBag.Schools = new SelectList(schools ?? new List<SchoolListDto>(), "Id", "Name");

            // Departmanları dinamik olarak yükle
            if (schoolId.HasValue)
            {
                var departments = await client.GetFromJsonAsync<List<DepartmentListDto>>(
                    $"api/admin/AdminSchool/{schoolId}/departments");
                ViewBag.Departments = new SelectList(departments ?? new List<DepartmentListDto>(), "Id", "Name");
            }
            else
            {
                ViewBag.Departments = new SelectList(new List<DepartmentListDto>(), "Id", "Name");
            }

            // Odaları çek
            var rooms = await client.GetFromJsonAsync<List<RoomDto>>("api/admin/AdminRoom");
            ViewBag.Rooms = new SelectList(rooms ?? new List<RoomDto>(), "Id", "Name");
        }

        #endregion
    }
}