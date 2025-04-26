//using AcademicAppointmentApi.BusinessLayer.Abstract;
//using AcademicAppointmentApi.EntityLayer.Entities;
//using AcademicAppointmentApi.Presentation.Dtos.DepartmentDtos;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace AcademicAppointmentApi.Presentation.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AdminDepartmentController : ControllerBase
//    {
//        private readonly IDepartmentService _departmentService;

//        public AdminDepartmentController(IDepartmentService departmentService)
//        {
//            _departmentService = departmentService;
//        }

//        // GET: api/admin/departments
//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            var list = await _departmentService.TGetAllAsync();
//            return Ok(list);
//        }

//        // GET: api/admin/departments/{id}
//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(string id)
//        {
//            var dept = await _departmentService.TGetByIdWithStringAsync(id);
//            if (dept == null) return NotFound();
//            return Ok(dept);
//        }

//        // GET: api/admin/departments/byschool/{schoolId}
//        [HttpGet("byschool/{schoolId}")]
//        public async Task<IActionResult> GetBySchool(string schoolId)
//        {

//            var deps = await _departmentService.GetDepartmentsBySchoolIdAsync(schoolId);
//            return Ok(deps);
//        }

//        // POST: api/admin/departments
//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] DepartmentCreateDto dto)
//        {

//            var entity = new Department
//            {
//                Id = Guid.NewGuid().ToString(),
//                Name = dto.Name,
//                SchoolId = dto.SchoolId
//            };

//            await _departmentService.TAddAsync(entity);
//            await _departmentService.TSaveAsync();

//            return CreatedAtAction(
//                nameof(GetById),
//                new { id = entity.Id },
//                entity
//            );
//        }

//        // PUT: api/admin/departments/{id}
//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(string id, [FromBody] DepartmentUpdateDto dto)
//        {
//            var existing = await _departmentService.TGetByIdWithStringAsync(id);
//            if (existing == null) return NotFound();

//            // Güncellenecek alanlar
//            existing.Name = dto.Name;
//            existing.SchoolId = dto.SchoolId;

//            await _departmentService.TUpdateAsync(existing);
//            await _departmentService.TSaveAsync();

//            return Ok(existing);
//        }

//        // DELETE: api/admin/departments/{id}
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(string id)
//        {
//            var existing = await _departmentService.TGetByIdWithStringAsync(id);
//            if (existing == null) return NotFound();

//            await _departmentService.TDeleteAsync(existing);
//            await _departmentService.TSaveAsync();

//            return NoContent();
//        }
//    }

//}
