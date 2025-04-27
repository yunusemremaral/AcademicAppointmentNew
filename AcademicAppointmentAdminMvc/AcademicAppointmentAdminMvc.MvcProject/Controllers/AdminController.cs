using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace AcademicAppointmentAdminMvc.MvcProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            // Claims'den username ve email bilgilerini alıyoruz
            ViewData["UserName"] = User.FindFirst("username")?.Value;
            ViewData["Email"] = User.FindFirst("email")?.Value;

            return View();
        }
    }




}
