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
    //[Authorize(AuthenticationSchemes = "Bearer")]
    //[Authorize(Roles = "Admin")]
    public class AdminCourseController : ControllerBase
    {
        private readonly ICourseService _courseService;
        private readonly IMapper _mapper;

        public AdminCourseController(ICourseService courseService, IMapper mapper)
        {
            _courseService = courseService;
            _mapper = mapper;
        }

        // GET: api/admin/AdminCourse
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _courseService.TGetAllAsync();
            var dto = _mapper.Map<List<CourseListDto>>(courses);
            return Ok(dto);
        }

        // GET: api/admin/AdminCourse/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var course = await _courseService.TGetByIdAsync(id);
            if (course == null) return NotFound();
            var dto = _mapper.Map<CourseListDto>(course);
            return Ok(dto);
        }

        // GET: api/admin/AdminCourse/WithDetails
        [HttpGet("WithDetails")]
        public async Task<IActionResult> GetAllWithDetails()
        {
            var courses = await _courseService.GetAllWithDetailsAsync();
            var dto = _mapper.Map<List<CourseWithInstructorAndDepartmentDto>>(courses);
            return Ok(dto);
        }

        // GET: api/admin/AdminCourse/WithDetails/5
        [HttpGet("WithDetails/{id}")]
        public async Task<IActionResult> GetByIdWithDetails(int id)
        {
            var course = await _courseService.GetByIdWithDetailsAsync(id);
            if (course == null) return NotFound();
            var dto = _mapper.Map<CourseDetailDto>(course);
            return Ok(dto);
        }

        // GET: api/admin/AdminCourse/ByInstructor/{instructorId}
        [HttpGet("ByInstructor/{instructorId}")]
        public async Task<IActionResult> GetAllByInstructor(string instructorId)
        {
            var courses = await _courseService.GetAllByInstructorIdWithDetailsAsync(instructorId);
            var dto = _mapper.Map<List<CourseWithInstructorAndDepartmentDto>>(courses);
            return Ok(dto);
        }

        // POST: api/admin/AdminCourse
        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateDto dto)
        {
            var course = _mapper.Map<Course>(dto);
            await _courseService.TAddAsync(course);
            return Ok(course);
        }

        // PUT: api/admin/AdminCourse
        [HttpPut]
        public async Task<IActionResult> Update(CourseUpdateDto dto)
        {
            var course = _mapper.Map<Course>(dto);
            await _courseService.TUpdateAsync(course);
            return Ok();
        }

        // DELETE: api/admin/AdminCourse/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _courseService.TGetByIdAsync(id);
            if (course == null) return NotFound();
            await _courseService.TDeleteAsync(course);
            return Ok();
        }
        // GET: api/admin/AdminCourse/WithFullDetails
        [HttpGet("WithFullDetails")]
        public async Task<IActionResult> GetAllWithFullDetails()
        {
            var courses = await _courseService.GetAllWithDetailsAsync(); // zaten detaylı veriyi çekiyor
            var dto = _mapper.Map<List<CourseWithFullDetailsDto>>(courses);
            return Ok(dto);
        }

    }
}