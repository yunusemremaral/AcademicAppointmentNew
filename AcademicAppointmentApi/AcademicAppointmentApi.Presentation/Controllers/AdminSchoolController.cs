using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentApi.Presentation.Dtos.School;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<IActionResult> AddSchool(SchoolCreateDto dto)
        {
            var school = new School { Name = dto.Name };
            await _schoolService.TAddAsync(school);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSchool(SchoolUpdateDto dto)
        {
            var school = await _schoolService.TGetByIdAsync(dto.Id);
            if (school == null)
                return NotFound();

            school.Name = dto.Name;
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
        [HttpGet("with-departments")]
        public async Task<IActionResult> GetAllSchoolsWithDepartments()
        {
            var schools = await _schoolService.TGetSchoolsWithDepartmentsAsync();
            return Ok(schools);
        }

    }
}
