using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminSchoolController : ControllerBase
    {
        private readonly ISchoolService _schoolService;

        public AdminSchoolController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSchools()
        {
            var schools = await _schoolService.TGetAllAsync();
            return Ok(schools);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchoolById(int id)
        {
            var school = await _schoolService.TGetByIdAsync(id);
            if (school == null)
                return NotFound();
            return Ok(school);
        }

        [HttpPost]
        public async Task<IActionResult> AddSchool(School school)
        {
            await _schoolService.TAddAsync(school);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSchool(School school)
        {
            await _schoolService.TUpdateAsync(school);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchool(int id)
        {
            var school = await _schoolService.TGetByIdAsync(id);
            if (school == null)
                return NotFound();

            await _schoolService.TDeleteAsync(school);
            return Ok();
        }
    }
}
