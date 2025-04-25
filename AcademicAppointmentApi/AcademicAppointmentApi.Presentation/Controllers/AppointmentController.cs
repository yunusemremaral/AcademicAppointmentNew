using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentApi.Presentation.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Student")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ICurrentUserService _currentUserService;

        public AppointmentController(IAppointmentService appointmentService, ICurrentUserService currentUserService)
        {
            _appointmentService = appointmentService;
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

            await _appointmentService.TAddAsync(appt);
            await _appointmentService.TSaveAsync();
            return Ok("Appointment created.");
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudent(string studentId)
        {
            var result = await _appointmentService.GetAppointmentsByStudentIdAsync(studentId);
            return Ok(result);
        }

        [HttpGet("academic/{academicId}")]
        public async Task<IActionResult> GetByAcademic(string academicId)
        {
            var result = await _appointmentService.GetAppointmentsByAcademicIdAsync(academicId);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _appointmentService.TGetByIdAsync(id);
            if (appointment == null)
                return NotFound();

            await _appointmentService.TDeleteAsync(appointment);
            await _appointmentService.TSaveAsync();
            return Ok("Appointment deleted.");
        }
    }
}