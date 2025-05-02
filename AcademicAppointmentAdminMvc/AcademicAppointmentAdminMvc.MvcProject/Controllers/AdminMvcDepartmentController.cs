using AcademicAppointmentAdminMvc.MvcProject.Models;
using AcademicAppointmentShare.Dtos.DepartmentDtos;
using AcademicAppointmentShare.Dtos.SchoolDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var response = await client.GetAsync("api/admin/AdminDepartment/with-school");

            if (!response.IsSuccessStatusCode)
                return View(new List<DepartmentWithSchoolDto>());

            var json = await response.Content.ReadAsStringAsync();
            var departments = JsonConvert.DeserializeObject<List<DepartmentWithSchoolDto>>(json);

            // Tüm okul adlarını al
            var schoolNames = departments.Select(d => d.SchoolName).Distinct().OrderBy(n => n).ToList();
            ViewBag.SchoolNames = new SelectList(schoolNames, schoolName);

            // Eğer filtre varsa, sadece o okula ait bölümler kalsın
            if (!string.IsNullOrEmpty(schoolName))
            {
                departments = departments.Where(d => d.SchoolName == schoolName).ToList();

                // SchoolId'yi ViewBag'e geçir (ilk denk gelen departmandan alabiliriz)
                var selectedSchoolId = departments.FirstOrDefault()?.SchoolId;
                ViewBag.SelectedSchoolId = selectedSchoolId;
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
        // Delete Action
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var client = CreateClient();
            var response = await client.DeleteAsync($"api/admin/admindepartment/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            // Hata durumunda
            var error = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, error);






            //// 2. Get departments with courses
            //public async Task<IActionResult> DepartmentsWithCourses()
            //{
            //    var client = CreateClient();
            //    var response = await client.GetAsync("api/admin/admindepartment/with-courses");

            //    if (!response.IsSuccessStatusCode)
            //    {
            //        return View("Error");
            //    }

            //    var jsonData = await response.Content.ReadAsStringAsync();
            //    var departments = JsonConvert.DeserializeObject<List<DepartmentListWithCoursesDto>>(jsonData);

            //    return View(departments);
            //}

            //// 3. Get courses by department ID
            //public async Task<IActionResult> CoursesByDepartment(int departmentId)
            //{
            //    var client = CreateClient();
            //    var response = await client.GetAsync($"api/admin/admindepartment/courses/{departmentId}");

            //    if (!response.IsSuccessStatusCode)
            //    {
            //        return View("Error");
            //    }

            //    var jsonData = await response.Content.ReadAsStringAsync();
            //    var courses = JsonConvert.DeserializeObject<List<DepartmentCourseDto>>(jsonData);

            //    return View(courses);
            //}

            //// 4. Get department details by ID
            //public async Task<IActionResult> DepartmentDetails(int id)
            //{
            //    var client = CreateClient();
            //    var response = await client.GetAsync($"api/admin/admindepartment/details/{id}");

            //    if (!response.IsSuccessStatusCode)
            //    {
            //        return View("Error");
            //    }

            //    var jsonData = await response.Content.ReadAsStringAsync();
            //    var department = JsonConvert.DeserializeObject<DepartmentDetailDto>(jsonData);

            //    return View(department);
            //}

            //// 5. Get the number of courses in a department
            //public async Task<IActionResult> CourseCount(int departmentId)
            //{
            //    var client = CreateClient();
            //    var response = await client.GetAsync($"api/admin/admindepartment/course-count/{departmentId}");

            //    if (!response.IsSuccessStatusCode)
            //    {
            //        return View("Error");
            //    }

            //    var jsonData = await response.Content.ReadAsStringAsync();
            //    var courseCount = JsonConvert.DeserializeObject<int>(jsonData);

            //    return View(courseCount);
            //}

            //// 6. Create a new department (Post method)
            //[HttpPost]
            //public async Task<IActionResult> CreateDepartment([FromBody] DepartmentCreateDto departmentCreateDto)
            //{
            //    if (departmentCreateDto == null)
            //        return BadRequest();

            //    var client = CreateClient();
            //    var content = new StringContent(JsonConvert.SerializeObject(departmentCreateDto), Encoding.UTF8, "application/json");
            //    var response = await client.PostAsync("api/admin/admindepartment", content);

            //    if (!response.IsSuccessStatusCode)
            //        return View("Error");

            //    return RedirectToAction("Index");
            //}

            //// 7. Update an existing department (Put method)
            //[HttpPut]
            //public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentUpdateDto departmentUpdateDto)
            //{
            //    if (id != departmentUpdateDto.Id)
            //        return BadRequest("ID mismatch.");

            //    var client = CreateClient();
            //    var content = new StringContent(JsonConvert.SerializeObject(departmentUpdateDto), Encoding.UTF8, "application/json");
            //    var response = await client.PutAsync($"api/admin/admindepartment/{id}", content);

            //    if (!response.IsSuccessStatusCode)
            //        return View("Error");

            //    return RedirectToAction("Index");
            //}

            //// 8. Delete a department
            //[HttpDelete]
            //public async Task<IActionResult> DeleteDepartment(int id)
            //{
            //    var client = CreateClient();
            //    var response = await client.DeleteAsync($"api/admin/admindepartment/{id}");

            //    if (!response.IsSuccessStatusCode)
            //        return View("Error");

            //    return RedirectToAction("Index");
            //}
        }
    }
}
