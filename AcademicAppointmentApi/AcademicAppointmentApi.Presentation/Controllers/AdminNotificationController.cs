using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.BusinessLayer.Concrete;
using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentShare.Dtos.NotificationDtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    //[Authorize(Roles = "Admin")]
    public class AdminNotificationController : ControllerBase
    {
        private readonly INotificationService _genericService;
        private readonly IMapper _mapper;

        public AdminNotificationController(INotificationService genericService, IMapper mapper)
        {
            _genericService = genericService;
            _mapper = mapper;
        }

        // Get All Notifications
        [HttpGet]
        public async Task<IActionResult> GetAllNotifications()
        {
            var notifications = await _genericService.TGetAllAsync();
            var notificationDtos = _mapper.Map<List<NotificationDto>>(notifications);
            return Ok(notificationDtos);
        }

        // Get Notification By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            var notification = await _genericService.TGetByIdAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            var notificationDto = _mapper.Map<NotificationDto>(notification);
            return Ok(notificationDto);
        }

        [HttpPost]
        public async Task<IActionResult> AddNotification([FromBody] CreateNotificationDto dto)
        {
            var entity = _mapper.Map<Notification>(dto);
            entity.CreatedAt = DateTime.UtcNow;
            entity.IsRead = false;
            var result = await _genericService.TAddAsync(entity);
            return Ok(_mapper.Map<NotificationDto>(result));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(int id, [FromBody] UpdateNotificationDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var entity = _mapper.Map<Notification>(dto);
            await _genericService.TUpdateAsync(entity);
            return NoContent();
        }

        // Delete Notification
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var notification = await _genericService.TGetByIdAsync(id);
            if (notification == null)
            {
                return NotFound();
            }
            await _genericService.TDeleteAsync(notification);
            return NoContent();
        }
    }
}
