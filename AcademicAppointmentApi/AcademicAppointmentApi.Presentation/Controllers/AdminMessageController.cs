using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.Presentation.Dtos.MessageDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/admin/messages")]
    public class AdminMessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public AdminMessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var messages = await _messageService.GetAllAsync();
            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MessageCreateDto dto)
        {
            await _messageService.CreateAsync(dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _messageService.TDeleteAsync(id);
            return Ok();
        }
    }

}
