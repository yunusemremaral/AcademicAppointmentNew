using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentShare.Dtos.UserDtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/admin/[controller]")]
[ApiController]
//[Authorize(AuthenticationSchemes = "Bearer")]
//[Authorize(Roles = "Admin")]
public class AdminUserController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public AdminUserController(UserManager<AppUser> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetAllUsers()
    {
        var users = _userManager.Users.ToList();
        var usersDto = _mapper.Map<List<UserDto>>(users);
        return Ok(usersDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(_mapper.Map<UserDto>(user));
    }

    [HttpGet("{id}/roles")]
    public async Task<IActionResult> GetUserRoles(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(roles);
    }

    [HttpPost("{id}/roles")]
    public async Task<IActionResult> AddUserToRole(string id, [FromBody] string role)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var result = await _userManager.AddToRoleAsync(user, role);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok();
    }

    [HttpDelete("{id}/roles/{role}")]
    public async Task<IActionResult> RemoveUserFromRole(string id, string role)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var result = await _userManager.RemoveFromRoleAsync(user, role);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok();
    }

    [HttpGet("by-role/{roleName}")]
    public async Task<IActionResult> GetUsersByRole(string roleName)
    {
        var users = await _userManager.GetUsersInRoleAsync(roleName);
        return Ok(_mapper.Map<List<UserDto>>(users));
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        var user = _mapper.Map<AppUser>(dto);
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok(_mapper.Map<UserDto>(user));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserDto dto)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        _mapper.Map(dto, user);
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok(_mapper.Map<UserDto>(user));
    }
    [HttpGet("{id}/details")]
    public async Task<IActionResult> GetUserDetails(string id)
    {
        var user = await _userManager.Users
            .Include(u => u.School)
            .Include(u => u.Department)
            .Include(u => u.Room)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null) return NotFound();

        var userDto = _mapper.Map<UserWithDetailsDto>(user);
        return Ok(userDto);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok();
    }
    [HttpGet("{id}/courses")]
    public async Task<IActionResult> GetUserCourses(string id)
    {
        var user = await _userManager.Users
            .Include(u => u.Courses)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null) return NotFound();

        var courseDtos = _mapper.Map<List<UserCourseDto>>(user.Courses);
        return Ok(courseDtos);
    }

    [HttpGet("by-role/{roleName}/with-department")]
    public async Task<IActionResult> GetUsersByRoleWithDepartment(string roleName)
    {
        var users = await _userManager.GetUsersInRoleAsync(roleName);
        var userDetails = users.Select(u => new
        {
            u.Id,
            u.UserName,
            u.Email,
            Department = u.Department?.Name,
            School = u.School?.Name
        }).ToList();

        return Ok(userDetails);
    }


}