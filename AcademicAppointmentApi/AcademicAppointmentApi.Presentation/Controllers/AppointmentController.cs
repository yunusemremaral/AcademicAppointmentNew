using AcademicAppointmentApi.DataAccessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentController(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            await _appointmentRepository.AddAsync(appointment);
            await _appointmentRepository.SaveAsync();
            return Ok("Randevu oluşturuldu");
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
            return Ok("Randevu silindi");
        }
    }

}
