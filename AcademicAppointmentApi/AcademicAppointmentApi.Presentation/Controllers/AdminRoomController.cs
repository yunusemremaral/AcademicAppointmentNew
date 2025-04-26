using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminRoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public AdminRoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRooms()
        {
            var rooms = await _roomService.TGetAllAsync();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var room = await _roomService.TGetByIdAsync(id);
            if (room == null)
                return NotFound();
            return Ok(room);
        }

        [HttpPost]
        public async Task<IActionResult> AddRoom(Room room)
        {
            await _roomService.TAddAsync(room);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRoom(Room room)
        {
            await _roomService.TUpdateAsync(room);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _roomService.TGetByIdAsync(id);
            if (room == null)
                return NotFound();

            await _roomService.TDeleteAsync(room);
            return Ok();
        }
    }
}
