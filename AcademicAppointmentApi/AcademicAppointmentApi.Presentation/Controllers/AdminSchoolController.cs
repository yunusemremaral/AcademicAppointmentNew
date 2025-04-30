using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentShare.Dtos.DepartmentDtos;
using AcademicAppointmentShare.Dtos.SchoolDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Roles = "Admin")]
    public class AdminSchoolController : ControllerBase
    {
        private readonly ISchoolService _schoolService;

        public AdminSchoolController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        // GET: api/admin/AdminSchool
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var schools = await _schoolService.TGetAllAsync();

            var dtoList = schools.Select(s => new SchoolListDto
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();

            return Ok(dtoList);
        }

        // GET: api/admin/AdminSchool/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var school = await _schoolService.TGetByIdAsync(id);
            if (school == null)
                return NotFound();

            var dto = new SchoolListDto
            {
                Id = school.Id,
                Name = school.Name
            };

            return Ok(dto);
        }

        // POST: api/admin/AdminSchool
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] SchoolCreateDto dto)
        {
            var school = new School
            {
                Name = dto.Name
            };

            var result = await _schoolService.TAddAsync(school);

            var responseDto = new SchoolListDto
            {
                Id = result.Id,
                Name = result.Name
            };

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, responseDto);
        }

        // PUT: api/admin/AdminSchool/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SchoolUpdateDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID uyuşmuyor.");

            var school = await _schoolService.TGetByIdAsync(id);
            if (school == null)
                return NotFound();

            school.Name = dto.Name;

            await _schoolService.TUpdateAsync(school);

            return NoContent();
        }

        // DELETE: api/admin/AdminSchool/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var school = await _schoolService.TGetByIdAsync(id);
            if (school == null)
                return NotFound();

            await _schoolService.TDeleteAsync(school);
            return NoContent();
        }

        // GET: api/admin/AdminSchool/with-departments
        [HttpGet("with-departments")]
        public async Task<IActionResult> GetAllWithDepartments()
        {
            var schools = await _schoolService.TGetAllWithDepartmentsAsync();

            var dtoList = schools.Select(s => new SchoolDetailDto
            {
                Id = s.Id,
                Name = s.Name,
                Departments = s.Departments?.Select(d => new DepartmentDto
                {
                    Id = d.Id,
                    Name = d.Name
                }).ToList() ?? new List<DepartmentDto>()
            }).ToList();

            return Ok(dtoList);
        }

        // GET: api/admin/AdminSchool/{schoolId}/departments
        [HttpGet("{schoolId}/departments")]
        public async Task<IActionResult> GetDepartmentsBySchoolId(int schoolId)
        {
            var departments = await _schoolService.TGetDepartmentsBySchoolIdAsync(schoolId);

            var dtoList = departments.Select(d => new DepartmentDto
            {
                Id = d.Id,
                Name = d.Name
            }).ToList();

            return Ok(dtoList);
        }

        // GET: api/admin/AdminSchool/{schoolId}/department-count
        [HttpGet("{schoolId}/department-count")]
        public async Task<IActionResult> GetDepartmentCount(int schoolId)
        {
            var count = await _schoolService.TGetDepartmentCountAsync(schoolId);
            return Ok(new { SchoolId = schoolId, DepartmentCount = count });
        }
    }
}
