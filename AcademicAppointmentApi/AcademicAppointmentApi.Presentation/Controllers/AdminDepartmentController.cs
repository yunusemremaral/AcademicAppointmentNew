using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminDepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public AdminDepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDepartments()
        {
            var departments = await _departmentService.TGetAllAsync();
            return Ok(departments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            var department = await _departmentService.TGetByIdAsync(id);
            if (department == null)
                return NotFound();
            return Ok(department);
        }

        [HttpPost]
        public async Task<IActionResult> AddDepartment(Department department)
        {
            await _departmentService.TAddAsync(department);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDepartment(Department department)
        {
            await _departmentService.TUpdateAsync(department);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _departmentService.TGetByIdAsync(id);
            if (department == null)
                return NotFound();

            await _departmentService.TDeleteAsync(department);
            return Ok();
        }
        [HttpGet("by-school/{schoolId}")]
        public async Task<IActionResult> GetDepartmentsBySchoolId(int schoolId)
        {
            var departments = await _departmentService.TGetDepartmentsBySchoolIdAsync(schoolId);
            if (departments == null || !departments.Any())
                return NotFound();

            return Ok(departments);
        }


    }
}
