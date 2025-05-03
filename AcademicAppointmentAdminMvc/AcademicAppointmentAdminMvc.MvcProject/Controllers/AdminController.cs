//using Microsoft.AspNetCore.Mvc;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Threading.Tasks;
//using Newtonsoft.Json;
//using AcademicAppointmentAdminMvc.MvcProject.Models;
//using Microsoft.AspNetCore.Authorization;
//using System.Security.Claims;

//namespace AcademicAppointmentMvc.MvcProject.Controllers
//{
//    [Authorize(Roles = "Admin")]
//    public class AdminController : Controller
//    {
//        private readonly IHttpClientFactory _httpClientFactory;
//        private readonly string _apiBaseUrl = "http://localhost:7214/api/AdminUser"; // API base URL'ini buraya koyuyorsunuz

//        public AdminController(IHttpClientFactory httpClientFactory)
//        {
//            _httpClientFactory = httpClientFactory;
//        }

//        public IActionResult Dashboard()
//        {
//            // Claims'den username, email, ve role bilgilerini alıyoruz
//            ViewData["UserName"] = User.FindFirst("username")?.Value;
//            ViewData["Email"] = User.FindFirst("email")?.Value;
//            ViewData["Role"] = User.FindFirst(ClaimTypes.Role)?.Value;

//            return View();
//        }

//        // Tüm kullanıcıları çekme
//        public async Task<IActionResult> GetAllUsers()
//        {
//            var client = _httpClientFactory.CreateClient();

//            // Token'ı al ve Authorization header'a ekle
//            var token = Request.Cookies["JwtToken"];
//            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

//            // API'ye istek at
//            var response = await client.GetAsync("https://localhost:7214/api/AdminUser/all");

//            if (response.IsSuccessStatusCode)
//            {
//                var jsonString = await response.Content.ReadAsStringAsync();
//                var users = JsonConvert.DeserializeObject<List<UserDto>>(jsonString); // UserDto'yu uygun şekilde oluşturduğunuzdan emin olun
//                return View(users);
//            }
//            else
//            {
//                // Hata durumunda uygun bir mesaj göster
//                ModelState.AddModelError("", "Kullanıcılar alınamadı.");
//                return View();
//            }
//        }
//    }
//}
