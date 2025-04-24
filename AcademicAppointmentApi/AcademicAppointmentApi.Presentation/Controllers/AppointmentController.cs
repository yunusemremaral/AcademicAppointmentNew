using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.DataAccessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentApi.Presentation.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Student")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly ICurrentUserService _currentUserService;

        public AppointmentController(IAppointmentRepository repo, ICurrentUserService currentUserService)
        {
            _appointmentRepository = repo;
            _currentUserService = currentUserService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] AppointmentCreateDto dto)
        {
            var studentId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(studentId))
                return Unauthorized();

            var appt = new Appointment
            {
                AcademicUserId = dto.AcademicUserId,
                StudentUserId = studentId,
                ScheduledAt = dto.ScheduledAt,
                Subject = dto.Subject,
                Description = dto.Description,
                Status = dto.Status
            };

            await _appointmentRepository.AddAsync(appt);
            await _appointmentRepository.SaveAsync();
            return Ok("Appointment created.");
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudent(string studentId)
        {
            var result = await _appointmentRepository.GetAppointmentsByStudentIdAsync(studentId);
            return Ok(result);
        }

        [HttpGet("academic/{academicId}")]
        public async Task<IActionResult> GetByAcademic(string academicId)
        {
            var result = await _appointmentRepository.GetAppointmentsByAcademicIdAsync(academicId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
                return NotFound();

            _appointmentRepository.Delete(appointment);
            await _appointmentRepository.SaveAsync();
            return Ok("Appointment deleted.");
        }

        [Authorize]
        [HttpGet("whoami")]
        public IActionResult WhoAmI()
        {
            return Ok(new
            {
                Authenticated = User.Identity.IsAuthenticated,
                NameIdentifier = _currentUserService.UserId,
                Email = _currentUserService.Email,
                Role = _currentUserService.Role
            });
        }
    }
}
