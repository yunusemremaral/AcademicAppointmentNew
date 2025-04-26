using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AdminAppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await _appointmentService.TGetAllAsync();
            return Ok(appointments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var appointment = await _appointmentService.TGetByIdAsync(id);
            if (appointment == null)
                return NotFound();
            return Ok(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> AddAppointment(Appointment appointment)
        {
            await _appointmentService.TAddAsync(appointment);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAppointment(Appointment appointment)
        {
            await _appointmentService.TUpdateAsync(appointment);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _appointmentService.TGetByIdAsync(id);
            if (appointment == null)
                return NotFound();

            await _appointmentService.TDeleteAsync(appointment);
            return Ok();
        }
    }
}
