using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminCourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public AdminCourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courseService.TGetAllAsync();
            return Ok(courses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCourseById(int id)
        {
            var course = await _courseService.TGetByIdAsync(id);
            if (course == null)
                return NotFound();
            return Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> AddCourse(Course course)
        {
            await _courseService.TAddAsync(course);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCourse(Course course)
        {
            await _courseService.TUpdateAsync(course);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await _courseService.TGetByIdAsync(id);
            if (course == null)
                return NotFound();

            await _courseService.TDeleteAsync(course);
            return Ok();
        }
    }
}
