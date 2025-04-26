using AcademicAppointmentApi.EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminRoleController : ControllerBase
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public AdminRoleController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // ✅ Tüm rolleri listele
        [HttpGet("all")]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }

        // ✅ Yeni rol oluştur
        [HttpPost("create")]
        public async Task<IActionResult> CreateRole([FromQuery] string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                return BadRequest("Bu rol zaten mevcut.");

            var result = await _roleManager.CreateAsync(new AppRole { Name = roleName });

            return result.Succeeded ? Ok("Rol oluşturuldu.") : BadRequest(result.Errors);
        }

        // ✅ Rolü sil
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteRole([FromQuery] string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                return NotFound("Rol bulunamadı.");

            var result = await _roleManager.DeleteAsync(role);

            return result.Succeeded ? Ok("Rol silindi.") : BadRequest(result.Errors);
        }

        // ✅ Rol adını güncelle
        [HttpPut("update")]
        public async Task<IActionResult> UpdateRole([FromQuery] string oldRoleName, [FromQuery] string newRoleName)
        {
            var role = await _roleManager.FindByNameAsync(oldRoleName);
            if (role == null)
                return NotFound("Eski rol bulunamadı.");

            role.Name = newRoleName;
            var result = await _roleManager.UpdateAsync(role);

            return result.Succeeded ? Ok("Rol güncellendi.") : BadRequest(result.Errors);
        }

        // ✅ Kullanıcıya rol ata
        [HttpPost("assignrole")]
        public async Task<IActionResult> AssignRoleToUser([FromQuery] string userId, [FromQuery] string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            var result = await _userManager.AddToRoleAsync(user, roleName);

            return result.Succeeded ? Ok("Rol atandı.") : BadRequest(result.Errors);
        }

        // ✅ Kullanıcıdan rol kaldır
        [HttpPost("removerole")]
        public async Task<IActionResult> RemoveRoleFromUser([FromQuery] string userId, [FromQuery] string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            return result.Succeeded ? Ok("Rol kaldırıldı.") : BadRequest(result.Errors);
        }

        // ✅ Kullanıcının rollerini listele
        [HttpGet("userroles")]
        public async Task<IActionResult> GetUserRoles([FromQuery] string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }
    }
}
