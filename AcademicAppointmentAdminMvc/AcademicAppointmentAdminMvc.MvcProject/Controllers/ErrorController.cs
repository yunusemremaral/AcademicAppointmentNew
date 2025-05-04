using Microsoft.AspNetCore.Mvc;

namespace AcademicAppointmentAdminMvc.MvcProject.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            ViewData["StatusCode"] = statusCode;

            switch (statusCode)
            {
                case 404:
                    ViewData["Message"] = "The page you’re looking for was not found.";
                    break;
                case 500:
                    ViewData["Message"] = "An internal server error occurred.";
                    break;
                default:
                    ViewData["Message"] = "An unexpected error occurred.";
                    break;
            }

            return View("Error");
        }
    }
}
