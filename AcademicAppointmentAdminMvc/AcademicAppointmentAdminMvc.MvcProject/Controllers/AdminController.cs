using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using AcademicAppointmentAdminMvc.MvcProject.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using AcademicAppointmentShare.Dtos.SchoolDtos;
using AcademicAppointmentShare.Dtos.UserDtos;

namespace AcademicAppointmentMvc.MvcProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminController(IHttpClientFactory httpClientFactory)
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
        public async Task<IActionResult> Dashboard()
        {
            var client = CreateClient();

            async Task<int> GetCountAsync(string url)
            {
                var response = await client.GetStringAsync(url);
                if (int.TryParse(response, out int result))
                {
                    return result;
                }

                try
                {
                    var countObj = JsonConvert.DeserializeObject<CountDto>(response);
                    return countObj?.Count ?? 0;
                }
                catch
                {
                    return 0;
                }
            }

            ViewBag.KullanıcıSayısı = await GetCountAsync("api/admin/AdminUser/count");
            ViewBag.OkulSayısı = await GetCountAsync("api/admin/AdminSchool/count");
            ViewBag.BölümSayısı = await GetCountAsync("api/admin/AdminDepartment/count");
            ViewBag.DersSayısı = await GetCountAsync("api/admin/AdminCourse/count");
            ViewBag.RandevuSayısı = await GetCountAsync("api/admin/AdminAppointment/count");
            ViewBag.ÖgretmenSayısı = await GetCountAsync("api/admin/AdminUser/instructor-count");
            ViewBag.ÖgrenciSayısı = await GetCountAsync("api/admin/AdminUser/student-count");
            ViewBag.MessageSayısı = await GetCountAsync("api/admin/AdminNotification/count");

            var responseStatus = await client.GetStringAsync("api/admin/AdminAppointment/appointment-status-counts");
            var statusCounts = JsonConvert.DeserializeObject<Dictionary<string, int>>(responseStatus);

            // Serialize for JavaScript
            ViewBag.StatusCountsJson = JsonConvert.SerializeObject(statusCounts);

            var responseDailyCounts = await client.GetStringAsync("api/admin/AdminAppointment/daily-appointment-counts");
            var dailyCounts = JsonConvert.DeserializeObject<Dictionary<string, int>>(responseDailyCounts);
            ViewBag.DailyCountsJson = JsonConvert.SerializeObject(dailyCounts);

            // Son 5 okul verisi
            var responseSchools = await client.GetStringAsync("api/admin/AdminSchool/latest-5-schools");
            var schools = JsonConvert.DeserializeObject<List<SchoolListDto>>(responseSchools);
            ViewBag.LatestSchools = schools;

            // Son 5 kullanıcı verisi
            var responseUsers = await client.GetStringAsync("api/admin/AdminUser/latest-5-users");
            var users = JsonConvert.DeserializeObject<List<UserDto>>(responseUsers);
            ViewBag.LatestUsers = users;

            return View();
        }




    }
}
