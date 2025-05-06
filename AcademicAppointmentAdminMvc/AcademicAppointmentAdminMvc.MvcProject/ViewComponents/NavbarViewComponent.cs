using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using AcademicAppointmentShare.Dtos.NotificationDtos;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using AcademicAppointmentShare.Dtos.UserDtos;

namespace AcademicAppointmentAdminMvc.MvcProject.ViewComponents
{
    public class NavbarViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NavbarViewComponent(IHttpClientFactory httpClientFactory)
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

        private UserDto GetUserInfoFromCookie()
        {
            var token = Request.Cookies["JwtToken"];
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            // Token'ı çözümleyerek kullanıcının bilgilerini al
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var username = jwtToken.Claims.FirstOrDefault(c => c.Type == "username")?.Value;
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
           

            return new UserDto
            {
                UserFullName = username,
                Email = email,
            };
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = CreateClient();

            // Son 5 bildirimi çekme
            var notificationResponse = await client.GetAsync("api/admin/AdminNotification/last5");
            List<NotificationDto> notifications = new List<NotificationDto>();

            if (notificationResponse.IsSuccessStatusCode)
            {
                var json = await notificationResponse.Content.ReadAsStringAsync();
                notifications = JsonSerializer.Deserialize<List<NotificationDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            // Kullanıcı bilgilerini al
            var userInfo = GetUserInfoFromCookie();

            // Verileri View'a göndermek
            var model = new NavbarViewModel
            {
                Notifications = notifications,
                UserInfo = userInfo
            };

            return View(model);
        }
    }

    public class NavbarViewModel
    {
        public List<NotificationDto> Notifications { get; set; }
        public UserDto UserInfo { get; set; }
    }

   
}
