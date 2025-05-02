using AcademicAppointmentShare.Dtos.SchoolDtos;
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

    // Index: Okulları ve Bölümleri listele
    public async Task<IActionResult> Index()
    {
        var client = CreateClient();
        var response = await client.GetAsync("api/admin/AdminSchool/with-departments");

        if (!response.IsSuccessStatusCode)
            return View(new List<SchoolDetailDto>());

        var jsonData = await response.Content.ReadAsStringAsync();
        var schools = JsonConvert.DeserializeObject<List<SchoolDetailDto>>(jsonData);
        return View(schools);
    }

    public async Task<IActionResult> Details(int id)
    {
        var client = CreateClient(); // HttpClient'ı oluşturuyoruz
        var response = await client.GetAsync($"api/admin/AdminSchool/{id}/details");

        if (!response.IsSuccessStatusCode)
            return NotFound();  // Eğer hata varsa 404 döndür

        var jsonData = await response.Content.ReadAsStringAsync();

        // JSON verisini doğru şekilde deserialize ediyoruz
        var school = JsonConvert.DeserializeObject<SchoolDetailDto>(jsonData);

        // Null kontrolü ekliyoruz
        if (school == null)
            return NotFound(); // Eğer okul bilgisi yoksa 404 döndür

        // Departments null ise boş liste atıyoruz
        if (school.Departments == null)
        {
            school.Departments = new List<SDepartmentSchoolDto>();  // Boş bir liste
        }

        return View(school);  // Okul detaylarını View'a gönderiyoruz
    }


    // Create: Okul ekleme formu ve işlemi
    public IActionResult Create()
    {
        var dto = new SchoolCreateDto(); // boş bir DTO oluşturuyoruz
        return View(dto); // DTO'yu view'a gönderiyoruz
    }

    [HttpPost]
    public async Task<IActionResult> Create(SchoolCreateDto dto)
    {
        if (ModelState.IsValid)
        {
            var client = CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/admin/AdminSchool", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError(string.Empty, "Bir hata oluştu.");
        }

        return View(dto);
    }

    // Edit: Okul güncelleme formu ve işlemi
    public async Task<IActionResult> Edit(int id)
    {
        var client = CreateClient();
        var response = await client.GetAsync($"api/admin/AdminSchool/{id}");

        if (!response.IsSuccessStatusCode)
            return NotFound();

        var jsonData = await response.Content.ReadAsStringAsync();
        var school = JsonConvert.DeserializeObject<SchoolUpdateDto>(jsonData);
        return View(school);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SchoolUpdateDto dto)
    {
        if (id != dto.Id)
            return BadRequest("ID uyuşmuyor.");

        if (ModelState.IsValid)
        {
            var client = CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"api/admin/AdminSchool/{id}", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction(nameof(Index));

            ModelState.AddModelError(string.Empty, "Bir hata oluştu.");
        }

        return View(dto);
    }

    // Delete: Okul silme işlemi
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var client = CreateClient();
        var response = await client.DeleteAsync($"api/admin/AdminSchool/{id}");

        if (response.IsSuccessStatusCode)
            return RedirectToAction(nameof(Index));

        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var error = await response.Content.ReadAsStringAsync();
            TempData["DeleteError"] = error;
            return RedirectToAction(nameof(Index));
        }

        TempData["DeleteError"] = "Bir hata oluştu. Okul silinemedi.";
        return RedirectToAction(nameof(Index));
    }

}

