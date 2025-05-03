using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentShare.Dtos.RoomDtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    //[Authorize(Roles = "Admin")]
    public class AdminRoomController : ControllerBase
    {
        private readonly IRoomService _roomService;
        private readonly IMapper _mapper;

        public AdminRoomController(IRoomService roomService, IMapper mapper)
        {
            _roomService = roomService;
            _mapper = mapper;
        }

        // GET: api/AdminRoom
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var rooms = await _roomService.TGetAllWithUsersAsync();
            var dtos = _mapper.Map<List<RoomDto>>(rooms);
            return Ok(dtos);
        }

        // GET: api/AdminRoom/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var room = await _roomService.TGetByIdWithUserAsync(id);
            if (room == null) return NotFound();
            var dto = _mapper.Map<RoomDto>(room);
            return Ok(dto);
        }
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetDetails(int id)
        {
            var room = await _roomService.TGetByIdWithUserAsync(id);
            if (room == null) return NotFound();

            var dto = _mapper.Map<RoomDetailDto>(room);
            return Ok(dto);
        }
        // GET: api/AdminRoom/by-user/{userId}
        [HttpGet("by-user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            var room = await _roomService.TGetByUserIdAsync(userId);
            if (room == null) return NotFound();
            var dto = _mapper.Map<RoomDto>(room);
            return Ok(dto);
        }

        // POST: api/AdminRoom
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoomDto createDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Manual mapping without AutoMapper
            var room = new Room
            {
                Name = createDto.Name,
                AppUserId = createDto.AppUserId, // Assuming AppUserId is in createDto
                                                 // Diğer gerekli alanlar varsa buraya ekleyebilirsiniz
            };

            try
            {
                var result = await _roomService.TAddAsync(room);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);  // Başarılı ekleme sonrası döndürülmesi
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = "An error occurred while saving the entity changes.",
                    details = ex.InnerException?.Message ?? ex.Message
                });
            }
        }



        // PUT: api/AdminRoom
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateRoomDto updateDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var room = await _roomService.TGetByIdAsync(updateDto.Id);
            if (room == null) return NotFound();

            _mapper.Map(updateDto, room);
            await _roomService.TUpdateAsync(room);
            return NoContent();
        }

        // DELETE: api/AdminRoom/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _roomService.TGetByIdAsync(id);
            if (room == null) return NotFound();

            await _roomService.TDeleteAsync(room);
            return NoContent();
        }
    }
}
