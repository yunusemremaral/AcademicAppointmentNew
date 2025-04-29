using AcademicAppointmentAdminMvc.MvcProject.Dtos.SchoolDtos;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

[Authorize(Roles = "Admin")]
public class AdminMvcSchoolController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AdminMvcSchoolController(IHttpClientFactory httpClientFactory)
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

    public async Task<IActionResult> Index()
    {
        var client = CreateClient();
        var response = await client.GetAsync("api/admin/AdminSchool/with-departments");
        var jsonData = await response.Content.ReadAsStringAsync();
        var values = JsonConvert.DeserializeObject<List<SchoolWithDepartmentsDto>>(jsonData);
        return View(values);
    }


    [HttpGet]
    public IActionResult AddSchool()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddSchool(SchoolCreateDto dto)
    {
        var client = CreateClient();
        var jsonData = JsonConvert.SerializeObject(dto);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        var response = await client.PostAsync("api/admin/AdminSchool", content);

        if (response.IsSuccessStatusCode)
            return RedirectToAction("Index");

        return View();
    }

    public async Task<IActionResult> DeleteSchool(int id)
    {
        var client = CreateClient();
        var response = await client.DeleteAsync($"/api/admin/AdminSchool/{id}");

        if (response.IsSuccessStatusCode)
            return RedirectToAction("Index");

        return View("Index");
    }

    [HttpGet]
    public async Task<IActionResult> UpdateSchool(int id)
    {
        var client = CreateClient();
        var response = await client.GetAsync($"/api/admin/AdminSchool/{id}");
        var jsonData = await response.Content.ReadAsStringAsync();
        var value = JsonConvert.DeserializeObject<SchoolUpdateDto>(jsonData);
        return View(value);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateSchool(SchoolUpdateDto dto)
    {
        var client = CreateClient();
        var jsonData = JsonConvert.SerializeObject(dto);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        var response = await client.PutAsync("/api/admin/AdminSchool", content);

        if (response.IsSuccessStatusCode)
            return RedirectToAction("Index");

        return View(dto);
    }
    public async Task<IActionResult> SchoolDepartments(int id)
    {
        var client = CreateClient();
        var response = await client.GetAsync($"api/admin/AdminSchool/with-departments");
        if (!response.IsSuccessStatusCode) return RedirectToAction("Index");

        var jsonData = await response.Content.ReadAsStringAsync();
        var allSchools = JsonConvert.DeserializeObject<List<SchoolWithDepartmentsDto>>(jsonData);

        var selectedSchool = allSchools.FirstOrDefault(s => s.Id == id);
        return View(selectedSchool);
    }
}
