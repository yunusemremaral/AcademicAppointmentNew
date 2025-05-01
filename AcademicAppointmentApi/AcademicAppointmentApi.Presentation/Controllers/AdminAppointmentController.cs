using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentShare.Dtos.AppointmentDtoS;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    //[Authorize(Roles = "Admin")]
    public class AdminAppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IMapper _mapper;

        public AdminAppointmentController(IAppointmentService appointmentService, IMapper mapper)
        {
            _appointmentService = appointmentService;
            _mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var appointments = await _appointmentService.TGetAllAppointmentsWithUsersAsync();
            var result = _mapper.Map<List<AppointmentResultDto>>(appointments);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var appointment = await _appointmentService.TGetAppointmentByIdWithUsersAsync(id);
            if (appointment == null) return NotFound();
            var result = _mapper.Map<AppointmentResultDto>(appointment);
            return Ok(result);
        }

        [HttpGet("academic/{academicId}")]
        public async Task<IActionResult> GetByAcademic(string academicId)
        {
            var appointments = await _appointmentService.TGetAppointmentsByAcademicIdAsync(academicId);
            var result = _mapper.Map<List<AppointmentResultDto>>(appointments);
            return Ok(result);
        }

        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudent(string studentId)
        {
            var appointments = await _appointmentService.TGetAppointmentsByStudentIdAsync(studentId);
            var result = _mapper.Map<List<AppointmentResultDto>>(appointments);
            return Ok(result);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(AppointmentStatus status)
        {
            var appointments = await _appointmentService.TGetAppointmentsByStatusAsync(status);
            var result = _mapper.Map<List<AppointmentResultDto>>(appointments);
            return Ok(result);
        }

        [HttpGet("date/{date}")]
        public async Task<IActionResult> GetByDate(DateTime date)
        {
            var appointments = await _appointmentService.TGetAppointmentsByDateAsync(date);
            var result = _mapper.Map<List<AppointmentResultDto>>(appointments);
            return Ok(result);
        }

        [HttpGet("range")]
        public async Task<IActionResult> GetInDateRange(DateTime start, DateTime end)
        {
            var appointments = await _appointmentService.TGetAppointmentsInDateRangeAsync(start, end);
            var result = _mapper.Map<List<AppointmentResultDto>>(appointments);
            return Ok(result);
        }

        [HttpGet("user/past/{userId}")]
        public async Task<IActionResult> GetPastAppointments(string userId)
        {
            var appointments = await _appointmentService.TGetPastAppointmentsByUserIdAsync(userId);
            var result = _mapper.Map<List<AppointmentResultDto>>(appointments);
            return Ok(result);
        }

        [HttpGet("user/upcoming/{userId}")]
        public async Task<IActionResult> GetUpcomingAppointments(string userId)
        {
            var appointments = await _appointmentService.TGetUpcomingAppointmentsByUserIdAsync(userId);
            var result = _mapper.Map<List<AppointmentResultDto>>(appointments);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppointmentCreateDto dto)
        {
            var appointment = _mapper.Map<Appointment>(dto);
            await _appointmentService.TAddAsync(appointment);
            return Ok("Appointment created.");
        }

        [HttpPut]
        public async Task<IActionResult> Update(AppointmentUpdateDto dto)
        {
            var appointment = await _appointmentService.TGetByIdAsync(dto.Id);
            if (appointment == null)
                return NotFound("Appointment not found.");

            appointment.ScheduledAt = dto.ScheduledAt;
            appointment.Subject = dto.Subject;
            appointment.Description = dto.Description;
            appointment.Status = (AppointmentStatus)dto.Status; // enum'a çeviriyoruz

            await _appointmentService.TUpdateAsync(appointment);
            return Ok("Appointment updated.");

        }
        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateStatus([FromBody] AppointmentStatusUpdateDto dto)
        {
            var appointment = await _appointmentService.TGetByIdAsync(dto.AppointmentId);
            if (appointment == null)    
                return NotFound("Appointment not found.");

            appointment.Status = (AppointmentStatus)dto.Status;
            await _appointmentService.TUpdateAsync(appointment);

            return Ok("Appointment status updated successfully.");
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _appointmentService.TGetByIdAsync(id);
            if (entity == null) return NotFound();
            await _appointmentService.TDeleteAsync(entity);
            return Ok("Appointment deleted.");
        }
    }
}
