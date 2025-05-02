using AcademicAppointmentAdminMvc.MvcProject.Models;
using AcademicAppointmentShare.Dtos.CourseDtos;
using AcademicAppointmentShare.Dtos.DepartmentDtos;
using AcademicAppointmentShare.Dtos.SchoolDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace AcademicAppointmentAdminMvc.MvcProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminMvcDepartmentController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminMvcDepartmentController(IHttpClientFactory httpClientFactory)
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

        public async Task<IActionResult> Index(string schoolName)
        {
            var client = CreateClient();

            // TÜM OKULLARI ÇEK (Bölümü olsun olmasın)
            var schoolsResponse = await client.GetAsync("api/admin/adminschool");
            if (!schoolsResponse.IsSuccessStatusCode)
                return View(new List<DepartmentWithSchoolDto>());

            var schools = JsonConvert.DeserializeObject<List<SchoolListDto>>(
                await schoolsResponse.Content.ReadAsStringAsync()
            );

            // BÖLÜMLERİ ÇEK (Okul bilgileriyle birlikte)
            var departmentsResponse = await client.GetAsync("api/admin/AdminDepartment/with-school");
            var departments = departmentsResponse.IsSuccessStatusCode
                ? JsonConvert.DeserializeObject<List<DepartmentWithSchoolDto>>(
                    await departmentsResponse.Content.ReadAsStringAsync())
                : new List<DepartmentWithSchoolDto>();

            // DROPDOWN İÇİN TÜM OKUL ADLARINI KULLAN (Bölümü olmasa bile)
            var schoolNames = schools.Select(s => s.Name).Distinct().OrderBy(n => n).ToList();
            ViewBag.SchoolNames = new SelectList(schoolNames, schoolName);

            // Filtreleme yap
            if (!string.IsNullOrEmpty(schoolName))
            {
                // Seçilen okulun ID'sini bul (schools listesinden)
                var selectedSchool = schools.FirstOrDefault(s => s.Name == schoolName);
                ViewBag.SelectedSchoolId = selectedSchool?.Id;

                // Departmanları filtrele (eğer bu okula ait departman yoksa liste boş kalır)
                departments = departments.Where(d => d.SchoolName == schoolName).ToList();
            }
            else
            {
                ViewBag.SelectedSchoolId = null;
            }

            return View(departments);
        }


        // Create Action
        [HttpGet]
        public async Task<IActionResult> Create(int schoolId)
        {
            var client = CreateClient();

            // Okulları dropdown için al
            var response = await client.GetAsync("api/admin/adminschool");
            if (!response.IsSuccessStatusCode) return View(new DepartmentCreateDto()); // Hata kontrolü

            var schoolList = JsonConvert.DeserializeObject<List<SchoolListDto>>(
                await response.Content.ReadAsStringAsync()
            );

            var model = new DepartmentCreateDto
            {
                SchoolId = schoolId // null olabilir, sorun değil
            };

            ViewBag.Schools = new SelectList(schoolList, "Id", "Name", model.SchoolId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DepartmentCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                // Okul listesi yeniden yüklenmeli
                var client = CreateClient();
                var schoolList = JsonConvert.DeserializeObject<List<SchoolListDto>>(
                    await (await client.GetAsync("api/admin/adminschool")).Content.ReadAsStringAsync()
                );
                ViewBag.Schools = new SelectList(schoolList, "Id", "Name", model.SchoolId);
                return View(model);
            }

            var response = await CreateClient().PostAsJsonAsync("api/admin/admindepartment", model);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            // Hata durumunda
            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", error);

            // Okul listesi tekrar yüklensin
            var schools = JsonConvert.DeserializeObject<List<SchoolListDto>>(
                await (await CreateClient().GetAsync("api/admin/adminschool")).Content.ReadAsStringAsync()
            );
            ViewBag.Schools = new SelectList(schools, "Id", "Name", model.SchoolId);

            return View(model);
        }

        private async Task LoadSchoolsForViewBag()
        {
            var client = CreateClient();
            var schoolsResponse = await client.GetAsync("api/admin/adminschool");
            if (schoolsResponse.IsSuccessStatusCode)
            {
                var schools = JsonConvert.DeserializeObject<List<SchoolListDto>>(
                    await schoolsResponse.Content.ReadAsStringAsync()
                );
                ViewBag.Schools = new SelectList(schools, "Id", "Name");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var client = CreateClient();

            // Departman detaylarını al (SchoolId için with-school endpointini kullanıyoruz)
            var response = await client.GetAsync("api/admin/AdminDepartment/with-school");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var departments = JsonConvert.DeserializeObject<List<DepartmentWithSchoolDto>>(
                await response.Content.ReadAsStringAsync()
            );

            var department = departments.FirstOrDefault(d => d.Id == id);
            if (department == null)
                return NotFound();

            // Update DTO'sunu oluştur
            var updateDto = new DepartmentUpdateDto
            {
                Id = department.Id,
                Name = department.DepartmentName,
                SchoolId = department.SchoolId
            };

            // Okul listesini yükle
            await LoadSchoolsForViewBag();
            return View(updateDto);
        }

        // Edit POST
        [HttpPost]
        public async Task<IActionResult> Edit(DepartmentUpdateDto model)
        {
            if (!ModelState.IsValid)
            {
                await LoadSchoolsForViewBag();
                return View(model);
            }

            var client = CreateClient();
            var response = await client.PutAsJsonAsync($"api/admin/AdminDepartment/{model.Id}", model);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            // Hata durumunda
            var error = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", error);
            await LoadSchoolsForViewBag();
            return View(model);
        }

        // Delete POST
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"api/admin/AdminDepartment/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = error;
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int departmentId)
        {
            var client = CreateClient();

            // API'ye istek atıyoruz
            var response = await client.GetAsync($"api/admin/AdminDepartment/by-department-full/{departmentId}");
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Dersler getirilemedi.";
                return RedirectToAction("Index");
            }

            // API'den dönen verileri DepartmentCourseWithInstructorDto'ya dönüştürüyoruz
            var courseList = JsonConvert.DeserializeObject<List<DepartmentCourseWithInstructorDto>>(
                await response.Content.ReadAsStringAsync()
            );
 
            ViewBag.DepartmentName = courseList.FirstOrDefault().Name;
            return View(courseList); // Detay view'ına yönlendir
        }





    }
}
