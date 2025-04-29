using AcademicAppointmentAdminMvc.MvcProject.Dtos.DepartmentDtos;
using AcademicAppointmentAdminMvc.MvcProject.Dtos.SchoolDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = CreateClient();
            var response = await client.GetAsync("api/admin/AdminDepartment");

            if (!response.IsSuccessStatusCode)
                return View(new List<GetDepartmentDto>());

            var jsonData = await response.Content.ReadAsStringAsync();
            var departments = JsonConvert.DeserializeObject<List<GetDepartmentDto>>(jsonData);
            return View(departments);
        }

        [HttpGet]
        public async Task<IActionResult> AddDepartment(int schoolId)
        {
            var client = CreateClient();
            var schoolResponse = await client.GetAsync("api/admin/AdminSchool");
            var schoolJson = await schoolResponse.Content.ReadAsStringAsync();
            var schoolList = JsonConvert.DeserializeObject<List<GetSchoolDto>>(schoolJson);

            ViewBag.Schools = schoolList;
            ViewBag.SchoolId = schoolId;

            return View();
        }
        [HttpGet]
        public async Task<IActionResult> SchoolDepartments(int schoolId)
        {
            var client = CreateClient();
            var response = await client.GetAsync($"api/admin/AdminDepartment/by-school/{schoolId}");

            if (!response.IsSuccessStatusCode)
                return View(new List<GetDepartmentDto>());

            var jsonData = await response.Content.ReadAsStringAsync();
            var departments = JsonConvert.DeserializeObject<List<GetDepartmentDto>>(jsonData);
            return View(departments);
        }

        [HttpPost]
        public async Task<IActionResult> AddDepartment(AddDepartmentDto dto)
        {
            var client = CreateClient();
            var jsonData = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/admin/AdminDepartment", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("SchoolDepartments", "AdminMvcSchool", new { id = dto.SchoolId });

            // Okullar tekrar yüklenmeli, hata durumunda da dropdown dolu olsun
            var schoolResponse = await client.GetAsync("api/admin/AdminSchool");
            var schoolJson = await schoolResponse.Content.ReadAsStringAsync();
            var schoolList = JsonConvert.DeserializeObject<List<GetSchoolDto>>(schoolJson);

            ViewBag.Schools = schoolList;
            ViewBag.SchoolId = dto.SchoolId;

            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateDepartment(int id)
        {
            var client = CreateClient();

            // Department verisi
            var response = await client.GetAsync($"api/admin/AdminDepartment/{id}");
            var jsonData = await response.Content.ReadAsStringAsync();
            var value = JsonConvert.DeserializeObject<UpdateDepartmentDto>(jsonData);

            // School verisi
            var schoolResponse = await client.GetAsync("api/admin/AdminSchool");
            var schoolJson = await schoolResponse.Content.ReadAsStringAsync();
            var schoolList = JsonConvert.DeserializeObject<List<GetSchoolDto>>(schoolJson);

            ViewBag.Schools = schoolList;

            return View(value);
        }
        private async Task<List<GetSchoolDto>> GetSchoolsAsync()
        {
            var client = CreateClient();
            var schoolResponse = await client.GetAsync("api/admin/AdminSchool");
            var schoolJson = await schoolResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<GetSchoolDto>>(schoolJson);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateDepartment(UpdateDepartmentDto dto)
        {
            var client = CreateClient();
            var jsonData = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("api/admin/AdminDepartment", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("SchoolDepartments", "AdminMvcSchool", new { id = dto.SchoolId });

            // Hata durumunda tekrar okul listesi yüklenmeli
            var schoolResponse = await client.GetAsync("api/admin/AdminSchool");
            var schoolJson = await schoolResponse.Content.ReadAsStringAsync();
            var schoolList = JsonConvert.DeserializeObject<List<GetSchoolDto>>(schoolJson);

            ViewBag.Schools = schoolList;

            return View(dto);
        }

        public async Task<IActionResult> DeleteDepartment(int id, int schoolId)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"api/admin/AdminDepartment/{id}");

            return RedirectToAction("SchoolDepartments", "AdminMvcSchool", new { id = schoolId });
        }
    }
}
