using AcademicAppointmentAdminMvc.MvcProject.Dtos.UserMvcDtos;
using AcademicAppointmentAdminMvc.MvcProject.Models;
using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentShare.Dtos.CourseDtos;
using AcademicAppointmentShare.Dtos.DepartmentDtos;
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



     


    }
}
