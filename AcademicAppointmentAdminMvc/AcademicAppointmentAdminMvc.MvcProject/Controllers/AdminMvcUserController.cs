using AcademicAppointmentAdminMvc.MvcProject.Models;
using AcademicAppointmentShare.Dtos.RoleDtos;
using AcademicAppointmentShare.Dtos.SchoolDtos;
using AcademicAppointmentShare.Dtos.UserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;

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
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return client;
        }

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

            if (!string.IsNullOrEmpty(roleFilter))
                users = await FilterByRole(users, roleFilter);

            if (!string.IsNullOrEmpty(searchQuery))
                users = users.Where(u =>
                    u.Email.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                    u.UserFullName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)
                ).ToList();

            ViewBag.Roles = await GetRoleList();
            return View(users);
        }

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

        public async Task<IActionResult> Create()
        {
            var dto = new CreateUserMvcDto();
            await PopulateDropdowns(dto);  
            return View(dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserMvcDto dto)
        {
            
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(dto);
                return View(dto);
            }

            var createDto = new CreateUserDto
            {
                UserFullName = dto.UserFullName,
                Email = dto.Email,
                Password = dto.Password,
                SchoolId = dto.SchoolId,
                DepartmentId = dto.DepartmentId,
            };

            var client = CreateClient();
            var response = await client.PostAsJsonAsync("/api/admin/AdminUser", createDto);

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Kullanıcı başarıyla oluşturuldu!";
                return RedirectToAction(nameof(Index));
            }

            var error = await response.Content.ReadAsStringAsync();
            TempData["Error"] = $"Hata: {error}";
            await PopulateDropdowns(dto);
            return View(dto);
        }


        // Controllers/AdminMvcUserController.cs

        public async Task<IActionResult> Edit(string id)
        {
            var client = CreateClient();

            // 1) Kullanıcı detayını al
            var userResp = await client.GetAsync($"/api/admin/AdminUser/{id}/details");
            if (!userResp.IsSuccessStatusCode)
            {
                TempData["Error"] = "Kullanıcı bulunamadı!";
                return RedirectToAction(nameof(Index));
            }
            var userDetail = await userResp.Content.ReadFromJsonAsync<UserWithDetailsDto>();

            // 2) Okul + bölüm listesini tek seferde çek
            var schools = await client.GetFromJsonAsync<List<SchoolDetailDto>>("api/admin/AdminSchool/with-departments");

            // 3) ViewModel’i oluştur
            var vm = new UserEditViewModel
            {
                User = new UpdateUserDto
                {
                    Id = userDetail.Id,
                    UserFullName = userDetail.UserFullName,
                    Email = userDetail.Email,
                    SchoolId = userDetail.SchoolId,
                    DepartmentId = userDetail.DepartmentId
                },
                SchoolsWithDepartments = schools
            };

            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, UserEditViewModel vm)
        {
            if (id != vm.User.Id)
                ModelState.AddModelError("", "ID uyuşmadı.");

            if (!ModelState.IsValid)
            {
                // hata varsa dropdown tekrar doldur
                vm.SchoolsWithDepartments = await CreateClient()
                    .GetFromJsonAsync<List<SchoolDetailDto>>("api/admin/AdminSchool/with-departments");
                return View(vm);
            }

            // API’ye PUT
            var client = CreateClient();
            var resp = await client.PutAsJsonAsync($"/api/admin/AdminUser/{id}", vm.User);
            if (resp.IsSuccessStatusCode)
            {
                TempData["Success"] = "Güncelleme başarılı.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Error"] = "Güncelleme sırasında hata: " + await resp.Content.ReadAsStringAsync();
            vm.SchoolsWithDepartments = await client
                .GetFromJsonAsync<List<SchoolDetailDto>>("api/admin/AdminSchool/with-departments");
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"/api/admin/AdminUser/{id}");

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Kullanıcı başarıyla silindi!";
            else
                TempData["Error"] = "Silme işlemi başarısız akademisyenin dersi mevcut !";

            // Redirect ile tekrar Index aksiyonuna gidiyoruz
            return RedirectToAction(nameof(Index));
        }

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
                UserFullName = user.UserFullName,
                AllRoles = roles.Select(r => new SelectListItem(r.Name, r.Name)).ToList(),
                AssignedRoles = userRoles
            };

            return View(model);
        }

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

        private async Task PopulateDropdowns(CreateUserMvcDto dto)
        {
            var client = CreateClient(); 
            // Okul ve Bölüm verilerini tek seferde çek
            var schoolsWithDepts = await client.GetFromJsonAsync<List<SchoolDetailDto>>("api/admin/AdminSchool/with-departments");
            dto.SchoolsWithDepartments = schoolsWithDepts;


        }


        #endregion
    }
}