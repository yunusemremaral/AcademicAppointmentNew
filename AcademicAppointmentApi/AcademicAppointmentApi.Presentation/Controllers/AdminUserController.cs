using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentApi.Presentation.Dtos;
using AcademicAppointmentApi.Presentation.Dtos.AdminUserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Roles = "Admin")]
    public class AdminUserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;

        public AdminUserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        // ✅ Tüm kullanıcıları listele
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users
                .Include(u => u.School)
                .Include(u => u.Department)
                .ToListAsync();

            var result = users.Select(u => new
            {
                u.Id,
                u.UserName,
                u.Email,
                School = u.School?.Name,
                Department = u.Department?.Name
            });

            return Ok(result);
        }

        // ✅ Kullanıcı oluştur
        [HttpPost("create")]
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

            return Ok("Kullanıcı başarıyla oluşturuldu.");
        }

        // ✅ Kullanıcıyı id ile sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded ? Ok("Kullanıcı silindi.") : BadRequest(result.Errors);
        }

        // ✅ Belirli bir role sahip kullanıcıları getir
        [HttpGet("byrole/{roleName}")]
        public async Task<IActionResult> GetUsersByRole(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            var result = users.Select(u => new
            {
                u.Id,
                u.UserName,
                u.Email
            });

            return Ok(result);
        }

        // ✅ Kullanıcının bilgilerini güncelle
        [HttpPut("update/{id}")]
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
    }
}
