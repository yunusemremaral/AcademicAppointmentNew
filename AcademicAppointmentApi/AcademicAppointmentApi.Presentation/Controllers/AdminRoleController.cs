using AcademicAppointmentApi.EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminRoleController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public AdminRoleController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // ✅ Tüm rolleri getir
        [HttpGet("roles")]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(roles);
        }

        // ✅ Yeni rol oluştur
        [HttpPost("createrole")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (await _roleManager.RoleExistsAsync(roleName))
                return BadRequest("Bu rol zaten mevcut.");

            var result = await _roleManager.CreateAsync(new AppRole { Name = roleName });
            return result.Succeeded ? Ok("Rol oluşturuldu.") : BadRequest(result.Errors);
        }

        // ✅ Rolü sil
        [HttpDelete("deleterole")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
                return NotFound("Rol bulunamadı.");

            var result = await _roleManager.DeleteAsync(role);
            return result.Succeeded ? Ok("Rol silindi.") : BadRequest(result.Errors);
        }

        // ✅ Rolü güncelle
        [HttpPut("updaterole")]
        public async Task<IActionResult> UpdateRole(string oldRoleName, string newRoleName)
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
        public async Task<IActionResult> AssignRole(string email, string role)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            var result = await _userManager.AddToRoleAsync(user, role);
            return result.Succeeded ? Ok("Rol atandı.") : BadRequest(result.Errors);
        }

        // ✅ Kullanıcıdan rol sil
        [HttpPost("removerole")]
        public async Task<IActionResult> RemoveRole(string email, string role)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            var result = await _userManager.RemoveFromRoleAsync(user, role);
            return result.Succeeded ? Ok("Rol kaldırıldı.") : BadRequest(result.Errors);
        }

        // ✅ Kullanıcının rollerini listele
        [HttpGet("userroles")]
        public async Task<IActionResult> GetUserRoles(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }
    }
}