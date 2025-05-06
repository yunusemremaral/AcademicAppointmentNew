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
        try
        {
            // 1. Boşsa DTO’daki ilişkili alanları null bırak (map etmeden önce)
            if (!dto.SchoolId.HasValue)
                dto.SchoolId = null;

            if (!dto.DepartmentId.HasValue)
                dto.DepartmentId = null;


            // 2. DTO’yu AppUser’a map et
            var user = _mapper.Map<AppUser>(dto);

            var emailUsernamePart = dto.Email.Split('@')[0];
            user.UserName = emailUsernamePart;

            // 3. CreateAsync çağır
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    Errors = result.Errors.Select(e => e.Description)
                });
            }

            // 4. Başarılıysa döndür
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }
        catch (Exception ex)
        {
            var message = ex.InnerException?.Message ?? ex.Message;
            return BadRequest(new
            {
                Message = "Kullanıcı oluşturulurken bir hata oluştu.",
                Details = message
            });
        }
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
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return Ok();

            // IdentityResult hatalarını dön
            return BadRequest(new
            {
                Errors = result.Errors.Select(e => e.Description)
            });
        }
        catch (Exception ex)
        {
            // Beklenmedik hata; inner exception varsa o mesajı, yoksa ex.Message'i al
            var message = ex.InnerException?.Message ?? ex.Message;
            return BadRequest(new
            {
                Message = "Kullanıcı silinirken bir hata oluştu.",
                Details = message
            });
        }
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
            u.UserFullName,
            u.Email,
            Department = u.Department?.Name,
            School = u.School?.Name
        }).ToList();

        return Ok(userDetails);
    }
    [HttpGet("instructors-by-department/{departmentId}")]
    public async Task<IActionResult> GetInstructorsByDepartment(int departmentId)
    {
        var usersInRole = await _userManager.GetUsersInRoleAsync("Instructor");

        var filtered = usersInRole
            .Where(u => u.DepartmentId == departmentId)
            .ToList();

        var dtoList = _mapper.Map<List<UserSimpleDto>>(filtered);
        return Ok(dtoList);
    }
    [HttpGet("student-by-department/{departmentId}")]
    public async Task<IActionResult> GetStudentByDepartment(int departmentId)
    {
        var usersInRole = await _userManager.GetUsersInRoleAsync("Student");

        var filtered = usersInRole
            .Where(u => u.DepartmentId == departmentId)
            .ToList();

        var dtoList = _mapper.Map<List<UserSimpleDto>>(filtered);
        return Ok(dtoList);
    }
    [HttpGet("without-room")]
    public async Task<IActionResult> GetUsersWithoutRoom()
    {
        var usersWithoutRoom = await _userManager.Users
            .Include(u => u.Room)
            .Where(u => u.Room == null)
            .ToListAsync();

        var dtoList = _mapper.Map<List<UserSimpleDto>>(usersWithoutRoom);
        return Ok(dtoList);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers(string schoolId = null, string departmentId = null, string roomId = null)
    {
        var query = _userManager.Users.AsQueryable();

        if (!string.IsNullOrEmpty(schoolId))
            query = query.Where(u => u.SchoolId.ToString() == schoolId);

        if (!string.IsNullOrEmpty(departmentId))
            query = query.Where(u => u.DepartmentId.ToString() == departmentId);

        if (!string.IsNullOrEmpty(roomId))
            query = query.Where(u => u.RoomId.ToString() == roomId);

        var users = query.ToList();
        var usersDto = _mapper.Map<List<UserDto>>(users);

        return Ok(usersDto);
    }
    // /api/admin/adminuser/instructor-count
    [HttpGet("instructor-count")]
    public async Task<IActionResult> GetInstructorCount()
    {
        var instructors = await _userManager.GetUsersInRoleAsync("Instructor");
        return Ok(new { Count = instructors.Count });
    }

    // /api/admin/adminuser/student-count
    [HttpGet("student-count")]
    public async Task<IActionResult> GetStudentCount()
    {
        var students = await _userManager.GetUsersInRoleAsync("Student");
        return Ok(new { Count = students.Count });
    }
    [HttpGet("count")]
    public IActionResult GetUserCount()
    {
        var count = _userManager.Users.Count();
        return Ok(new { Count = count });
    }
    [HttpGet("latest-5-users")]
    public async Task<IActionResult> GetLatest5Users()
    {
        // Son 5 kullanıcıyı GUID sırasına göre almak
        var users = await _userManager.Users
            .OrderByDescending(u => u.Id)  // GUID ile ters sıralama
            .Take(5)  // Son 5 kullanıcıyı al
            .ToListAsync();

        var usersDto = _mapper.Map<List<UserDto>>(users);
        return Ok(usersDto);
    }



}