// Presentation/Controllers/AdminSchoolController.cs
using System;
using System.Threading.Tasks;
using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentApi.Presentation.Dtos;
using AcademicAppointmentApi.Presentation.Dtos.School;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin/schools")]
    public class AdminSchoolController : ControllerBase
    {
        private readonly IGenericService<School> _schoolService;

        public AdminSchoolController(IGenericService<School> schoolService)
        {
            _schoolService = schoolService;
        }

        // GET: api/admin/schools
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _schoolService.TGetAllAsync();
            return Ok(list);
        }

        // POST: api/admin/schools
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SchoolCreateDto dto)
        {
            var entity = new School
            {
                Id = Guid.NewGuid().ToString(),
                Name = dto.Name
            };

            await _schoolService.TAddAsync(entity);
            await _schoolService.TSaveAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = entity.Id },
                entity
            );
        }

        // PUT: api/admin/schools/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] SchoolUpdateDto dto)
        {
            var existing = await _schoolService.TGetByIdWithStringAsync(id);
            if (existing == null)
                return NotFound();

            existing.Name = dto.Name;
            await _schoolService.TUpdateAsync(existing);
            await _schoolService.TSaveAsync();

            return Ok(existing);
        }

        // GET: api/admin/schools/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var school = await _schoolService.TGetByIdWithStringAsync(id);
            if (school == null)
                return NotFound();
            return Ok(school);
        }

        

        // DELETE: api/admin/schools/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await _schoolService.TGetByIdWithStringAsync(id);
            if (existing == null)
                return NotFound();

            await _schoolService.TDeleteAsync(existing);
            await _schoolService.TSaveAsync();

            return NoContent();
        }
    }
}
