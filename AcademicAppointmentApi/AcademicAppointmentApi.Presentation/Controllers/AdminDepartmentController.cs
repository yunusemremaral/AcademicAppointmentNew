using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentApi.Presentation.Dtos.DepartmentDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Roles = "Admin")]
    public class AdminDepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public AdminDepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _departmentService.TGetDepartmentsWithSchoolAsync();

            var departmentDtos = departments.Select(x => new GetDepartmentDto
            {
                Id = x.Id,
                Name = x.Name,
                SchoolId = x.SchoolId,
                SchoolName = x.School?.Name
            }).ToList();

            return Ok(departmentDtos);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var department = await _departmentService.TGetByIdAsync(id);
            if (department == null)
                return NotFound();

            var dto = new UpdateDepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                SchoolId = department.SchoolId
            };

            return Ok(dto);
        }


        [HttpPost]
        public async Task<IActionResult> AddDepartment(DepartmentCreateDto dto)
        {
            var department = new Department
            {
                Name = dto.Name,
                SchoolId = dto.SchoolId
            };

            await _departmentService.TAddAsync(department);
            return Ok();
        }


        [HttpPut]
        public async Task<IActionResult> UpdateDepartment(UpdateDepartmentDto dto)
        {
            var department = await _departmentService.TGetByIdAsync(dto.Id);
            if (department == null)
                return NotFound();

            department.Name = dto.Name;
            department.SchoolId = dto.SchoolId;

            await _departmentService.TUpdateAsync(department);
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _departmentService.TGetByIdAsync(id);
            if (department == null)
                return NotFound();

            await _departmentService.TDeleteAsync(department);
            return Ok();
        }
        [HttpGet("by-school/{schoolId}")]
        public async Task<IActionResult> GetDepartmentsBySchoolId(int schoolId)
        {
            var departments = await _departmentService.TGetDepartmentsBySchoolIdAsync(schoolId);
            if (departments == null || !departments.Any())
                return NotFound();

            return Ok(departments);
        }


    }
}
