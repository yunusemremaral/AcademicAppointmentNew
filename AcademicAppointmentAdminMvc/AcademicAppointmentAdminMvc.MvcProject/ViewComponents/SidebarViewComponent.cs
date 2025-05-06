using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using AcademicAppointmentShare.Dtos.UserDtos;

namespace AcademicAppointmentAdminMvc.MvcProject.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        private UserDto GetUserInfoFromCookie()
        {
            var token = Request.Cookies["JwtToken"];
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var username = jwtToken.Claims.FirstOrDefault(c => c.Type == "userfullname")?.Value;
            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;

            return new UserDto
            {
                UserFullName = username,
                Email = email
            };
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userInfo = GetUserInfoFromCookie();
            return View("Default", userInfo);
        }
    }
}
