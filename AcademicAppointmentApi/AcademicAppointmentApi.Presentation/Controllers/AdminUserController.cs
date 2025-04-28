using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentApi.Presentation.Dtos.AdminUserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer", Roles = "Admin")]
    public class AdminUserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public AdminUserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        // ✅ Tüm kullanıcıları listele
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users
                .Include(u => u.School)
                .Include(u => u.Department)
                .ToListAsync();

            var userDtos = users.Select(u => new AdminUserListDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                SchoolName = u.School?.Name,
                DepartmentName = u.Department?.Name
            }).ToList();

            return Ok(userDtos);
        }

        // ✅ ID'ye göre kullanıcı getir
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.Users
                .Include(u => u.School)
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            var userDto = new AdminUserDetailDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                SchoolId = user.SchoolId,
                DepartmentId = user.DepartmentId,
                RoomId = user.RoomId,
                SchoolName = user.School?.Name,
                DepartmentName = user.Department?.Name
            };

            return Ok(userDto);
        }

        // ✅ Yeni kullanıcı oluştur
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto dto)
        {
            var user = new AppUser
            {
                UserName = dto.UserName,
                Email = dto.Email,
                SchoolId = dto.SchoolId,
                DepartmentId = dto.DepartmentId,
                RoomId = dto.RoomId
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Eğer rol atanacaksa
            if (!string.IsNullOrEmpty(dto.Role))
            {
                await _userManager.AddToRoleAsync(user, dto.Role);
            }

            return Ok("Kullanıcı başarıyla oluşturuldu.");
        }

        // ✅ Kullanıcı güncelle
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            user.UserName = dto.UserName ?? user.UserName;
            user.Email = dto.Email ?? user.Email;
            user.SchoolId = dto.SchoolId ?? user.SchoolId;
            user.DepartmentId = dto.DepartmentId ?? user.DepartmentId;
            user.RoomId = dto.RoomId ?? user.RoomId;

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded ? Ok("Kullanıcı güncellendi.") : BadRequest(result.Errors);
        }

        // ✅ Kullanıcıyı sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded ? Ok("Kullanıcı silindi.") : BadRequest(result.Errors);
        }

        // ✅ Belirli role sahip kullanıcıları getir
        [HttpGet("role/{roleName}")]
        public async Task<IActionResult> GetUsersByRole(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);

            var userDtos = users.Select(u => new AdminUserListDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email
            }).ToList();

            return Ok(userDtos);
        }

        // ✅ Kullanıcıya rol ata
        [HttpPost("{id}/role")]
        public async Task<IActionResult> AssignRole(string id, AssignRoleDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            var result = await _userManager.AddToRoleAsync(user, dto.Role);

            return result.Succeeded ? Ok("Rol atandı.") : BadRequest(result.Errors);
        }
    }
}
