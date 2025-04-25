using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.Presentation.Dtos.CourseDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/admin/courses")]
    public class AdminCourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public AdminCourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var courses = await _courseService.TGetAllAsync();
            return Ok(courses);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CourseCreateDto dto)
        {
            await _courseService.TAddAsync(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] CourseCreateDto dto)
        {
            await _courseService.TUpdateAsync(await _courseService.TGetByIdWithStringAsync(id));
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            
            await _courseService.TDeleteAsync(await _courseService.TGetByIdWithStringAsync(id));
            return Ok();
        }
    }

}
