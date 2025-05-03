using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentShare.Dtos.RoleDtos;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    //[Authorize(Roles = "Admin")]
    public class AdminRoleController : ControllerBase
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;  // UserManager'ı ekliyoruz
        private readonly IMapper _mapper;

        public AdminRoleController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;  // UserManager'ı inject ediyoruz
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return Ok(_mapper.Map<List<RoleDto>>(roles));
        }

        [HttpGet("{roleName}")]
        public async Task<IActionResult> GetRoleByName(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return NotFound();
            return Ok(_mapper.Map<RoleDto>(role));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName)) return BadRequest("Role name is required.");
            var result = await _roleManager.CreateAsync(new AppRole { Name = roleName });
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok();
        }

        [HttpDelete("{roleName}")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) return NotFound();

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok();
        }
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRoleToUser([FromBody] RoleAssignmentDto model)
        {
            if (string.IsNullOrWhiteSpace(model.UserId) || string.IsNullOrWhiteSpace(model.RoleName))
                return BadRequest("UserId and RoleName are required.");

            // Kullanıcıyı bul
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound("User not found.");

            // Rolü bul
            var role = await _roleManager.FindByNameAsync(model.RoleName);
            if (role == null) return NotFound("Role not found.");

            // Kullanıcıya rolü ata
            var result = await _userManager.AddToRoleAsync(user, role.Name);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok($"Role {model.RoleName} assigned to user {model.UserId}.");
        }
    }
}
