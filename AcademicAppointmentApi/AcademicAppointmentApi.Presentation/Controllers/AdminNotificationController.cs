using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminNotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public AdminNotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotifications()
        {
            var notifications = await _notificationService.TGetAllAsync();
            return Ok(notifications);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            var notification = await _notificationService.TGetByIdAsync(id);
            if (notification == null)
                return NotFound();
            return Ok(notification);
        }

        [HttpPost]
        public async Task<IActionResult> AddNotification(Notification notification)
        {
            await _notificationService.TAddAsync(notification);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNotification(Notification notification)
        {
            await _notificationService.TUpdateAsync(notification);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var notification = await _notificationService.TGetByIdAsync(id);
            if (notification == null)
                return NotFound();

            await _notificationService.TDeleteAsync(notification);
            return Ok();
        }
    }
}
