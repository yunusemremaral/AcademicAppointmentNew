using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentApi.Presentation.Dtos.CourseDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Roles = "Admin")]

    public class AdminCourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public AdminCourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        // Tüm dersleri getir
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _courseService.TGetCoursesWithDepartmentAndInstructorAsync();

            var dtoList = courses.Select(c => new GetCourseDto
            {
                Id = c.Id,
                Name = c.Name,
                DepartmentId = c.DepartmentId,
                DepartmentName = c.Department?.Name,
                InstructorId = c.InstructorId,
                InstructorName = $"{c.Instructor?.UserName}"
            }).ToList();

            return Ok(dtoList);
        }

        // ID ile ders getir
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _courseService.TGetByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        // Department'a göre dersleri getir
        [HttpGet("ByDepartment/{departmentId}")]
        public async Task<IActionResult> GetByDepartmentId(int departmentId)
        {
            var result = await _courseService.TGetCoursesByDepartmentIdAsync(departmentId);
            return Ok(result);
        }

        // Instructor'a göre dersleri getir
        [HttpGet("ByInstructor/{instructorId}")]
        public async Task<IActionResult> GetByInstructorId(string instructorId)
        {
            var result = await _courseService.TGetCoursesByInstructorIdAsync(instructorId);
            return Ok(result);
        }

        // Detaylı (Department ve Instructor dahil) dersleri getir
        [HttpGet("WithDetails")]
        public async Task<IActionResult> GetWithDetails()
        {
            var result = await _courseService.TGetCoursesWithDepartmentAndInstructorAsync();
            return Ok(result);
        }

        // Yeni ders ekle
        [HttpPost]
        public async Task<IActionResult> Add(Course course)
        {
            await _courseService.TAddAsync(course);
            return Ok();
        }

        // Ders güncelle
        [HttpPut]
        public async Task<IActionResult> Update(Course course)
        {
            await _courseService.TUpdateAsync(course);
            return Ok();
        }

        // Ders sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _courseService.TGetByIdAsync(id);
            if (course == null) return NotFound();

            await _courseService.TDeleteAsync(course);
            return Ok();
        }
    }
}
