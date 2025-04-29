using AcademicAppointmentAdminMvc.MvcProject.Dtos.CourseDtos;
using AcademicAppointmentAdminMvc.MvcProject.Dtos.DepartmentDtos;
using AcademicAppointmentAdminMvc.MvcProject.Dtos.UserMvcDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = CreateClient();
            var response = await client.GetAsync("api/AdminCourse");

            if (!response.IsSuccessStatusCode)
                return View(new List<GetCourseDto>());

            var jsonData = await response.Content.ReadAsStringAsync();
            var courses = JsonConvert.DeserializeObject<List<GetCourseDto>>(jsonData);
            return View(courses);
        }

        [HttpGet]
        public async Task<IActionResult> AddCourse()
        {
            var client = CreateClient();

            // Departmanları al
            var departmentResponse = await client.GetAsync("api/AdminDepartment");
            var departmentJson = await departmentResponse.Content.ReadAsStringAsync();
            var departmentList = JsonConvert.DeserializeObject<List<GetDepartmentDto>>(departmentJson);
            ViewBag.Departments = departmentList;

            // Eğitmenleri al
            var instructorResponse = await client.GetAsync("api/AdminUser/instructors");
            var instructorJson = await instructorResponse.Content.ReadAsStringAsync();
            var instructorList = JsonConvert.DeserializeObject<List<UserDto>>(instructorJson);
            ViewBag.Instructors = instructorList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse(AddCourseDto dto)
        {
            var client = CreateClient();
            var jsonData = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/AdminCourse", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(dto); // Hata durumunda geri döner
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCourse(int id)
        {
            var client = CreateClient();

            // Course bilgisi
            var response = await client.GetAsync($"api/AdminCourse/{id}");
            var courseJson = await response.Content.ReadAsStringAsync();
            var course = JsonConvert.DeserializeObject<UpdateCourseDto>(courseJson);

            // Departmanlar
            var departmentResponse = await client.GetAsync("api/AdminDepartment");
            var departmentJson = await departmentResponse.Content.ReadAsStringAsync();
            var departmentList = JsonConvert.DeserializeObject<List<GetDepartmentDto>>(departmentJson);
            ViewBag.Departments = departmentList;

            // Eğitmenler
            var instructorResponse = await client.GetAsync("api/AdminUser/instructors");
            var instructorJson = await instructorResponse.Content.ReadAsStringAsync();
            var instructorList = JsonConvert.DeserializeObject<List<UserDto>>(instructorJson);
            ViewBag.Instructors = instructorList;

            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCourse(UpdateCourseDto dto)
        {
            var client = CreateClient();
            var jsonData = JsonConvert.SerializeObject(dto);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("api/AdminCourse", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            return View(dto); // Hata varsa View'a geri dön
        }

        public async Task<IActionResult> DeleteCourse(int id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"api/AdminCourse/{id}");
            return RedirectToAction("Index");
        }
    }
}
