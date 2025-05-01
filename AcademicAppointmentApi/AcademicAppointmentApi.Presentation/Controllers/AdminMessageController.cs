using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentShare.Dtos.MessageDtos;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    //[Authorize(Roles = "Admin")]
    public class AdminMessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        public AdminMessageController(IMessageService messageService, IMapper mapper)
        {
            _messageService = messageService;
            _mapper = mapper;
        }

        // Tüm mesajlar
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var messages = await _messageService.TGetAllAsync();
            var dto = _mapper.Map<List<ResultMessageDto>>(messages);
            return Ok(dto);
        }

        // ID'ye göre mesaj
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var message = await _messageService.TGetByIdAsync(id);
            if (message == null) return NotFound();
            var dto = _mapper.Map<ResultMessageDto>(message);
            return Ok(dto);
        }

        // Yeni mesaj oluştur
        [HttpPost]
        public async Task<IActionResult> Create(CreateMessageDto dto)
        {
            var message = _mapper.Map<Message>(dto);
            await _messageService.TAddAsync(message);
            return Ok("Mesaj gönderildi.");
        }

        // Mesaj güncelle
        [HttpPut]
        public async Task<IActionResult> Update(UpdateMessageDto dto)
        {
            var message = await _messageService.TGetByIdAsync(dto.Id);
            if (message == null) return NotFound();

            message.Content = dto.Content;
            await _messageService.TUpdateAsync(message);
            return Ok("Mesaj güncellendi.");
        }

        // Mesaj sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var message = await _messageService.TGetByIdAsync(id);
            if (message == null) return NotFound();

            await _messageService.TDeleteAsync(message);
            return Ok("Mesaj silindi.");
        }

        // Kullanıcıya ait tüm mesajlar
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetMessagesByUserId(string userId)
        {
            var messages = await _messageService.TGetMessagesByUserIdAsync(userId);
            var dto = _mapper.Map<List<ResultMessageDto>>(messages);
            return Ok(dto);
        }

        // Kullanıcılar arası konuşma
        [HttpGet("conversation")]
        public async Task<IActionResult> GetConversation([FromQuery] string userId1, [FromQuery] string userId2)
        {
            var messages = await _messageService.TGetConversationAsync(userId1, userId2);
            var dto = _mapper.Map<List<ResultMessageDto>>(messages);
            return Ok(dto);
        }

        // Gönderilen mesajlar
        [HttpGet("sent/{senderId}")]
        public async Task<IActionResult> GetSentMessages(string senderId)
        {
            var messages = await _messageService.TGetSentMessagesAsync(senderId);
            var dto = _mapper.Map<List<ResultMessageDto>>(messages);
            return Ok(dto);
        }

        // Alınan mesajlar
        [HttpGet("received/{receiverId}")]
        public async Task<IActionResult> GetReceivedMessages(string receiverId)
        {
            var messages = await _messageService.TGetReceivedMessagesAsync(receiverId);
            var dto = _mapper.Map<List<ResultMessageDto>>(messages);
            return Ok(dto);
        }
        [HttpGet("with-relations")]
        public async Task<IActionResult> GetAllWithRelations()
        {
            var messages = await _messageService.TGetAllWithRelationsAsync();
            var dto = _mapper.Map<List<ResultMessageDto>>(messages);
            return Ok(dto);
        }
    }
}