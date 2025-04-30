using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentShare.Dtos.CourseDtos;
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
    public class AdminCourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;

        // Constructor dependency injection for both CourseService and AutoMapper
        public AdminCourseController(ICourseService courseService, IMapper mapper)
        {
            _courseService = courseService;
            _mapper = mapper;
        }

        // GET: api/admin/course
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _courseService.TGetAllAsync();
            var courseDtos = _mapper.Map<List<CourseListDto>>(courses);
            return Ok(courseDtos);
        }

        // GET: api/admin/course/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _courseService.TGetByIdAsync(id);
            if (course == null) return NotFound();
            var courseDto = _mapper.Map<CourseDetailDto>(course);
            return Ok(courseDto);
        }

        // GET: api/admin/course/by-department/{departmentId}
        [HttpGet("by-department/{departmentId}")]
        public async Task<IActionResult> GetByDepartmentId(int departmentId)
        {
            var courses = await _courseService.TGetByDepartmentIdAsync(departmentId);
            var courseDtos = _mapper.Map<List<CourseListDto>>(courses);
            return Ok(courseDtos);
        }

        // GET: api/admin/course/by-instructor/{instructorId}
        [HttpGet("by-instructor/{instructorId}")]
        public async Task<IActionResult> GetByInstructorId(string instructorId)
        {
            var courses = await _courseService.TGetByInstructorIdAsync(instructorId);
            var courseDtos = _mapper.Map<List<CourseListDto>>(courses);
            return Ok(courseDtos);
        }

        // GET: api/admin/course/details/{courseId}
        [HttpGet("details/{courseId}")]
        public async Task<IActionResult> GetCourseWithDetails(int courseId)
        {
            var course = await _courseService.TGetCourseWithDetailsAsync(courseId);
            if (course == null) return NotFound();
            var courseDto = _mapper.Map<CourseDetailDto>(course);
            return Ok(courseDto);
        }

        // POST: api/admin/course
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseCreateDto dto)
        {
            var course = _mapper.Map<Course>(dto);
            var createdCourse = await _courseService.TAddAsync(course);
            var createdCourseDto = _mapper.Map<CourseDetailDto>(createdCourse);
            return CreatedAtAction(nameof(GetById), new { id = createdCourse.Id }, createdCourseDto);
        }

        // PUT: api/admin/course/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CourseUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest("ID mismatch.");

            var course = _mapper.Map<Course>(dto);
            await _courseService.TUpdateAsync(course);
            return NoContent();
        }

        // DELETE: api/admin/course/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _courseService.TGetByIdAsync(id);
            if (course == null) return NotFound();

            await _courseService.TDeleteAsync(course);
            return NoContent();
        }
    }
}
