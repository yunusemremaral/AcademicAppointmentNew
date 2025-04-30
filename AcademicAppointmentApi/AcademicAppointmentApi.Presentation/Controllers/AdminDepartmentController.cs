using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentShare.Dtos.DepartmentDtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        private readonly IMapper _mapper;

        public AdminDepartmentController(IDepartmentService departmentService, IMapper mapper)
        {
            _departmentService = departmentService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var departments = await _departmentService.TGetAllAsync();
            var dto = _mapper.Map<List<DepartmentListDto>>(departments);
            return Ok(dto);
        }

        [HttpGet("with-courses")]
        public async Task<IActionResult> GetAllWithCourses()
        {
            var departments = await _departmentService.TGetAllWithCoursesAsync();
            var dto = _mapper.Map<List<DepartmentListWithCoursesDto>>(departments);
            return Ok(dto);
        }

        [HttpGet("by-school/{schoolId}")]
        public async Task<IActionResult> GetDepartmentsBySchoolId(int schoolId)
        {
            var departments = await _departmentService.TGetDepartmentsBySchoolIdAsync(schoolId);
            var dto = _mapper.Map<List<DepartmentListDto>>(departments);
            return Ok(dto);
        }

        [HttpGet("courses/{departmentId}")]
        public async Task<IActionResult> GetCoursesByDepartmentId(int departmentId)
        {
            var courses = await _departmentService.TGetCoursesByDepartmentIdAsync(departmentId);

            // AutoMapper kullanarak Course listesini DTO'ya dönüştürme
            var courseDtos = _mapper.Map<List<CourseWithDetailsDto>>(courses);

            return Ok(courseDtos);
        }


        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetByIdWithDetails(int id)
        {
            var department = await _departmentService.TGetByIdWithDetailsAsync(id);
            if (department == null) return NotFound();
            var dto = _mapper.Map<DepartmentDetailDto>(department);
            return Ok(dto);
        }

        // GET: api/admin/department/course-count/5
        [HttpGet("course-count/{departmentId}")]
        public async Task<IActionResult> GetCourseCount(int departmentId)
        {
            var count = await _departmentService.TGetCourseCountAsync(departmentId);
            return Ok(count);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DepartmentCreateDto dto)
        {
            var entity = _mapper.Map<Department>(dto);
            var created = await _departmentService.TAddAsync(entity);
            var createdDto = _mapper.Map<DepartmentUpdateDto>(created);
            return CreatedAtAction(nameof(GetByIdWithDetails), new { id = created.Id }, createdDto);
        }


        // PUT: api/admin/department/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DepartmentUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest();
            var entity = _mapper.Map<Department>(dto);
            await _departmentService.TUpdateAsync(entity);
            return NoContent();
        }

        // DELETE: api/admin/department/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _departmentService.TGetByIdAsync(id);
            if (department == null) return NotFound();
            await _departmentService.TDeleteAsync(department);
            return NoContent();
        }
    }
}
