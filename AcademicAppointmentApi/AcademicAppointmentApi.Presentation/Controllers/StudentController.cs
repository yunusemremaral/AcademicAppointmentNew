using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;

        public StudentController(ILogger<StudentController> logger)
        {
            _logger = logger;
        }
        [Authorize(Roles = "Student")]  // Sadece "Student" rolüne sahip kullanıcılar erişebilir
        [HttpGet("profile")]
        public IActionResult GetStudentProfile()
        {
            // JWT'den kullanıcı bilgilerini al
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;

            // JWT'nin içeriğini görmek için loglama yapabiliriz
            _logger.LogInformation($"User ID: {userId}, Email: {userEmail}");

            return Ok(new { UserId = userId, Email = userEmail });
        }
    }
}
