using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminMessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public AdminMessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMessages()
        {
            var messages = await _messageService.TGetAllAsync();
            return Ok(messages);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMessageById(int id)
        {
            var message = await _messageService.TGetByIdAsync(id);
            if (message == null)
                return NotFound();
            return Ok(message);
        }

        [HttpPost]
        public async Task<IActionResult> AddMessage(Message message)
        {
            await _messageService.TAddAsync(message);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMessage(Message message)
        {
            await _messageService.TUpdateAsync(message);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _messageService.TGetByIdAsync(id);
            if (message == null)
                return NotFound();

            await _messageService.TDeleteAsync(message);
            return Ok();
        }
    }
}
