using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentApi.Presentation.Dtos.AdminUserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/admin/[controller]")]
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

    // API: AdminUserController.cs (GetAllUsers Metodu)
    [HttpGet]
    public async Task<IActionResult> GetAllUsers([FromQuery] string role = null)
    {
        var query = _userManager.Users
            .Include(u => u.School)
            .Include(u => u.Department)
            .AsQueryable();

        if (!string.IsNullOrEmpty(role))
        {
            var usersInRole = await _userManager.GetUsersInRoleAsync(role);
            query = query.Where(u => usersInRole.Contains(u));
        }

        // Materialize the query with ToListAsync()
        var userDtos = await query.Select(u => new AdminUserListDto
        {
            Id = u.Id,
            UserName = u.UserName,
            Email = u.Email,
            SchoolName = u.School != null ? u.School.Name : "",
            DepartmentName = u.Department != null ? u.Department.Name : "",
            Roles = _userManager.GetRolesAsync(u).Result.ToList()
        }).ToListAsync(); // ✅ ToListAsync() ekleyin

        return Ok(userDtos);
    }
    // Kullanıcı detayları
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _userManager.Users
            .Include(u => u.School)
            .Include(u => u.Department)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return NotFound();

        var roles = await _userManager.GetRolesAsync(user);

        var userDto = new AdminUserDetailDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            SchoolId = user.SchoolId,
            DepartmentId = user.DepartmentId,
            RoomId = user.RoomId,
            SchoolName = user.School?.Name,
            DepartmentName = user.Department?.Name,
            Roles = roles.ToList()
        };

        return Ok(userDto);
    }

    // Kullanıcıya rol ata
    [HttpPost("{id}/roles")]
    public async Task<IActionResult> AssignRole(string id, AssignRoleDto dto)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        var result = await _userManager.AddToRoleAsync(user, dto.Role);
        return result.Succeeded ? Ok() : BadRequest(result.Errors);
    }

    // Kullanıcıdan rol kaldır
    [HttpDelete("{id}/roles")]
    public async Task<IActionResult> RemoveRole(string id, AssignRoleDto dto)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        var result = await _userManager.RemoveFromRoleAsync(user, dto.Role);
        return result.Succeeded ? Ok() : BadRequest(result.Errors);
    }
}