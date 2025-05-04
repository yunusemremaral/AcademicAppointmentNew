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
    //[Authorize(AuthenticationSchemes = "Bearer")]
    //[Authorize(Roles = "Admin")]
    public class AdminDepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;

        public AdminDepartmentController(IDepartmentService departmentService, IMapper mapper)
        {
            _departmentService = departmentService;
            _mapper = mapper;
        }

        // Get all departments
        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _departmentService.TGetAllAsync();
            var departmentDtos = _mapper.Map<List<DepartmentListDto>>(departments);
            return Ok(departmentDtos); 
        }

        // Get all departments with their courses
        [HttpGet("with-courses")]
        public async Task<IActionResult> GetAllDepartmentsWithCourses()
        {
            var departments = await _departmentService.TGetAllWithCoursesAsync();
            var departmentDtos = _mapper.Map<List<DepartmentListWithCoursesDto>>(departments);
            return Ok(departmentDtos);

        }
        // Get courses by department ID
        [HttpGet("courses/{departmentId}")]
        public async Task<IActionResult> GetCoursesByDepartmentId(int departmentId)
        {
            var courses = await _departmentService.TGetCoursesByDepartmentIdAsync(departmentId);
            var courseDtos = _mapper.Map<List<DepartmentCourseDto>>(courses); 
            return Ok(courseDtos);
        }

        // Get department details by ID
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetDepartmentDetailsById(int id)
        {
            var department = await _departmentService.TGetByIdWithDetailsAsync(id);
            if (department == null)
                return NotFound(); 

            var departmentDto = _mapper.Map<DepartmentDetailDto>(department);
            return Ok(departmentDto); 
        }

        // Get the number of courses in a department
        [HttpGet("course-count/{departmentId}")]
        public async Task<IActionResult> GetCourseCount(int departmentId)
        {
            var courseCount = await _departmentService.TGetCourseCountAsync(departmentId);
            return Ok(courseCount);
        }

        // Create a new department
        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] DepartmentCreateDto departmentCreateDto)
        {
            if (departmentCreateDto == null)
                return BadRequest(); 

            var department = _mapper.Map<Department>(departmentCreateDto);
            var createdDepartment = await _departmentService.TAddAsync(department);
            var createdDepartmentDto = _mapper.Map<DepartmentCreateDto>(createdDepartment);

            return CreatedAtAction(nameof(GetDepartmentDetailsById), new { id = createdDepartment.Id }, createdDepartmentDto);
        }

        // Update an existing department
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentUpdateDto departmentUpdateDto)
        {
            if (id != departmentUpdateDto.Id)
                return BadRequest("ID uyuşmuyor.");

            var existingDepartment = await _departmentService.TGetByIdAsync(id);
            if (existingDepartment == null)
                return NotFound("Böyle bir bölüm bulunamadı.");

            _mapper.Map(departmentUpdateDto, existingDepartment);
            await _departmentService.TUpdateAsync(existingDepartment);

            return NoContent();
        }

        // Delete a department
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _departmentService.TGetByIdAsync(id);
            if (department == null)
                return NotFound(); 

            await _departmentService.TDeleteAsync(department);
            return NoContent(); 
        }
        // GET: api/admin/admindepartment/with-school
        [HttpGet("with-school")]
        public async Task<IActionResult> GetDepartmentsWithSchool()
        {
            var departments = await _departmentService.TGetDepartmentsWithSchoolAsync();

            var departmentDtos = _mapper.Map<List<DepartmentWithSchoolDto>>(departments);
            return Ok(departmentDtos);
        }

        [HttpGet("by-department-full/{departmentId}")]
        public async Task<IActionResult> GetCoursesWithInstructor(int departmentId)
        {
            try
            {
                var courses = await _departmentService.TGetCoursesWithInstructorByDepartmentIdAsync(departmentId);

                // Map the courses to DTOs
                var courseDtos = _mapper.Map<List<DepartmentCourseWithInstructorDto>>(courses);

                return Ok(courseDtos);
            }
            catch (AutoMapperMappingException ex)
            {
                // Log or return the error details
                return BadRequest($"Mapping Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log or return the general error details
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet("count")]
        public async Task<IActionResult> GetDepartmentCount()
        {
            var count = await _departmentService.TCountAsync();
            return Ok(count);
        }



    }

}
