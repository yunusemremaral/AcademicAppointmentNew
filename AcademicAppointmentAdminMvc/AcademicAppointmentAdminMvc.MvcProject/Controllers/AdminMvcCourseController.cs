using AcademicAppointmentAdminMvc.MvcProject.Models;
using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentShare.Dtos.CourseDtos;
using AcademicAppointmentShare.Dtos.DepartmentDtos;
using AcademicAppointmentShare.Dtos.SchoolDtos;
using AcademicAppointmentShare.Dtos.UserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AcademicAppointmentAdminMvc.MvcProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminMvcCourseController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminMvcCourseController(IHttpClientFactory httpClientFactory)
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
        public async Task<IActionResult> Index(int? schoolId, int? departmentId, string instructorId)

        {
            var client = CreateClient();
            var res = await client.GetAsync("api/admin/AdminCourse/WithFullDetails");
            if (!res.IsSuccessStatusCode)
                return View(new CourseWithFullDetailsViewModel());

            var all = JsonConvert
                .DeserializeObject<List<CourseWithFullDetailsDto>>(await res.Content.ReadAsStringAsync());

            // 1) Tek LINQ zinciriyle filtreleme
            var filtered = all
                .Where(c =>
                    (!schoolId.HasValue || c.SchoolId == schoolId.Value) &&
                    (!departmentId.HasValue || c.DepartmentId == departmentId.Value) &&
                    (string.IsNullOrEmpty(instructorId) || c.InstructorId == instructorId)
                )
                .ToList();

            // 2) Dropdown verilerini gruplayarak al
            var schools = all
                .GroupBy(c => new { c.SchoolId, c.SchoolName })
                .Select(g => new SelectListItem(g.Key.SchoolName, g.Key.SchoolId.ToString()))
                .OrderBy(x => x.Text)
                .ToList();

            var departments = all
                .Where(c => !schoolId.HasValue || c.SchoolId == schoolId.Value)
                .GroupBy(c => new { c.DepartmentId, c.DepartmentName })
                .Select(g => new SelectListItem(g.Key.DepartmentName, g.Key.DepartmentId.ToString()))
                .OrderBy(x => x.Text)
                .ToList();

            var instructors = all
                .Where(c =>
                    (!schoolId.HasValue || c.SchoolId == schoolId.Value) &&
                    (!departmentId.HasValue || c.DepartmentId == departmentId.Value)
                )
                .GroupBy(c => new { c.InstructorId, c.InstructorUserName, c.InstructorEmail })
                .Select(g => new SelectListItem(
                    $"{g.Key.InstructorUserName} ({g.Key.InstructorEmail})",
                    g.Key.InstructorId
                ))
                .OrderBy(x => x.Text)
                .ToList();

            var vm = new CourseWithFullDetailsViewModel
            {
                Courses = filtered,
                Schools = new SelectList(schools, "Value", "Text", schoolId),
                Departments = new SelectList(departments, "Value", "Text", departmentId),
                Instructors = new SelectList(instructors, "Value", "Text", instructorId),
                SelectedSchoolId = schoolId,
                SelectedDepartmentId = departmentId,
                SelectedInstructorId = instructorId
            };

            return View(vm);
        }
        // GET: AdminMvcCourseController/Create
        // CREATE İŞLEMLERİ
        public async Task<IActionResult> Create()
        {
            var client = CreateClient();
            var vm = new CourseCreateMvcDto();

            try
            {
                // Okulları yükle
                var schoolsResponse = await client.GetAsync("api/admin/AdminSchool");
                if (schoolsResponse.IsSuccessStatusCode)
                {
                    var schools = JsonConvert.DeserializeObject<List<SchoolListDto>>(
                        await schoolsResponse.Content.ReadAsStringAsync());
                    vm.Schools = new SelectList(schools, "Id", "Name");
                }
            }

            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Okullar yüklenirken hata: {ex.Message}");
            }
            

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateMvcDto mvcDto)
        {
            var client = CreateClient();

            var apiDto = new CourseCreateDto
            {
                Name = mvcDto.Name,
                DepartmentId = mvcDto.DepartmentId,
                InstructorId = mvcDto.InstructorId
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(apiDto),
                Encoding.UTF8,
                "application/json"
            );

            try
            {
                var response = await client.PostAsync("api/admin/AdminCourse", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index");

                TempData["CreateError"] = "Lütfen Ders ismini, Bölümü, Okulu ve Eğitmeni giriniz.";
                return RedirectToAction("Create"); // GET metoduna yönlendir
            }
            catch (HttpRequestException)
            {
                TempData["CreateError"] = "Sunucuya bağlanılamadı. Lütfen daha sonra tekrar deneyiniz.";
                return RedirectToAction("Create");
            }
        }


        private async Task<IActionResult> ReloadDropdownsAndReturnView(CourseCreateMvcDto dto)
        {
            var client = CreateClient();

            // Okulları yeniden yükle
            try
            {
                var schoolsResponse = await client.GetAsync("api/admin/AdminSchool/GetAll"); // Düzeltildi
                if (schoolsResponse.IsSuccessStatusCode)
                {
                    var schools = JsonConvert.DeserializeObject<List<SchoolListDto>>(
                        await schoolsResponse.Content.ReadAsStringAsync());
                    dto.Schools = new SelectList(schools, "Id", "Name", dto.SchoolId);
                }
                else
                {
                    ModelState.AddModelError("", "Okullar yüklenemedi!");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Okul yükleme hatası: {ex.Message}");
            }

            // Departmentları yükle
            if (dto.SchoolId > 0)
            {
                try
                {
                    var deptResponse = await client.GetAsync($"api/admin/AdminSchool/{dto.SchoolId}/departments");
                    if (deptResponse.IsSuccessStatusCode)
                    {
                        var departments = JsonConvert.DeserializeObject<List<SDepartmentSchoolDto>>(
                            await deptResponse.Content.ReadAsStringAsync());
                        dto.Departments = new SelectList(departments, "Id", "Name", dto.DepartmentId);
                    }
                }
                catch { /* Hata yutulur */ }
            }

            // Instructorları yükle
            if (dto.DepartmentId > 0)
            {
                try
                {
                    var instructorsResponse = await client.GetAsync(
                        $"api/admin/AdminCourse/instructors-by-department/{dto.DepartmentId}");
                    if (instructorsResponse.IsSuccessStatusCode)
                    {
                        var instructors = JsonConvert.DeserializeObject<List<UserSimpleDto>>(
                            await instructorsResponse.Content.ReadAsStringAsync());
                        dto.Instructors = new SelectList(
                            instructors.Select(i => new {
                                Id = i.Id,
                                DisplayText = $"{i.UserName} ({i.Email})"
                            }),
                            "Id",
                            "DisplayText",
                            dto.InstructorId
                        );
                    }
                }
                catch { /* Hata yutulur */ }
            }

            return View(dto);
        }

        // AJAX ENDPOINT'LER
        [HttpGet]
        public async Task<JsonResult> GetDepartmentsBySchool(int schoolId)
        {
            var client = CreateClient();
            try
            {
                var response = await client.GetAsync($"api/admin/AdminSchool/{schoolId}/departments");
                if (response.IsSuccessStatusCode)
                {
                    var departments = JsonConvert.DeserializeObject<List<SDepartmentSchoolDto>>(
                        await response.Content.ReadAsStringAsync());
                    return Json(departments.Select(d => new { value = d.Id, text = d.Name }));
                }
                return Json(new List<object>());
            }
            catch
            {
                return Json(new List<object>());
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetInstructorsByDepartment(int departmentId)
        {
            var client = CreateClient();
            try
            {
                var response = await client.GetAsync(
                    $"api/admin/AdminUser/instructors-by-department/{departmentId}");
                if (response.IsSuccessStatusCode)
                {
                    var instructors = JsonConvert.DeserializeObject<List<UserSimpleDto>>(
                        await response.Content.ReadAsStringAsync());
                    return Json(instructors.Select(i => new {
                        value = i.Id,
                        text = $"{i.UserName} ({i.Email})" // Format düzeltildi
                    }));
                }
                return Json(new List<object>());
            }
            catch
            {
                return Json(new List<object>());
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"api/admin/AdminCourse/{id}");

            if (!response.IsSuccessStatusCode) return NotFound();

            var jsonString = await response.Content.ReadAsStringAsync();
            var course = JsonConvert.DeserializeObject<CourseGetByIdDto>(jsonString);

            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CourseGetByIdDto model)
        {
            var client = CreateClient();

            var updateDto = new CourseUpdateDto
            {
                Id = model.Id,
                Name = model.Name,
                DepartmentId = model.DepartmentId,
                InstructorId = model.InstructorId
            };

            var content = new StringContent(JsonConvert.SerializeObject(updateDto), Encoding.UTF8, "application/json");
            var response = await client.PutAsync("api/admin/AdminCourse", content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Güncelleme başarısız oldu.");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"api/admin/AdminCourse/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Ders başarıyla silindi.";
            }
            else
            {
                TempData["Error"] = "Silme işlemi sırasında bir hata oluştu.";
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"api/admin/AdminCourse/WithDetails/{id}");

            if (!response.IsSuccessStatusCode)
                return NotFound();

            var jsonString = await response.Content.ReadAsStringAsync();
            var course = JsonConvert.DeserializeObject<CourseDetailDto>(jsonString);

            return View(course);
        }










    }
}
