//using AcademicAppointmentApi.BusinessLayer.Abstract;
//using AcademicAppointmentApi.EntityLayer.Entities;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Threading.Tasks;

//namespace AcademicAppointmentApi.Presentation.Controllers
//{
//    [Route("api/admin/[controller]")]
//    [ApiController]
//    [Authorize(AuthenticationSchemes = "Bearer")]
//    [Authorize(Roles = "Admin")]
//    public class AdminDepartmentController : ControllerBase
//    {
//        private readonly IDepartmentService _departmentService;

//        public AdminDepartmentController(IDepartmentService departmentService)
//        {
//            _departmentService = departmentService;
//        }

//        // ---------- GENERIC METOTLAR ----------

//        [HttpGet("get-all")]
//        public async Task<IActionResult> GetAll()
//        {
//            var departments = await _departmentService.TGetAllAsync();
//            return Ok(departments);
//        }

//        [HttpGet("get-by-id/{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var department = await _departmentService.TGetByIdAsync(id);
//            if (department == null) return NotFound();
//            return Ok(department);
//        }

//        [HttpPost("add")]
//        public async Task<IActionResult> Add([FromBody] Department department)
//        {
//            var result = await _departmentService.TAddAsync(department);
//            return Created("", result);
//        }

//        [HttpPut("update")]
//        public async Task<IActionResult> Update([FromBody] Department department)
//        {
//            await _departmentService.TUpdateAsync(department);
//            return NoContent();
//        }

//        [HttpDelete("delete/{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var department = await _departmentService.TGetByIdAsync(id);
//            if (department == null) return NotFound();

//            await _departmentService.TDeleteAsync(department);
//            return NoContent();
//        }

//        // ---------- SPESİFİK METOTLAR ----------

//        [HttpGet("get-all-with-courses")]
//        public async Task<IActionResult> GetAllWithCourses()
//        {
//            var result = await _departmentService.TGetAllWithCoursesAsync();
//            return Ok(result);
//        }

//        [HttpGet("get-all-with-faculty-members")]
//        public async Task<IActionResult> GetAllWithFacultyMembers()
//        {
//            var result = await _departmentService.TGetAllWithFacultyMembersAsync();
//            return Ok(result);
//        }

//        [HttpGet("get-all-with-students")]
//        public async Task<IActionResult> GetAllWithStudents()
//        {
//            var result = await _departmentService.TGetAllWithStudentsAsync();
//            return Ok(result);
//        }

//        [HttpGet("get-by-school-id/{schoolId}")]
//        public async Task<IActionResult> GetDepartmentsBySchoolId(int schoolId)
//        {
//            var result = await _departmentService.TGetDepartmentsBySchoolIdAsync(schoolId);
//            return Ok(result);
//        }

//        [HttpGet("search-by-name")]
//        public async Task<IActionResult> SearchByName([FromQuery] string name)
//        {
//            var result = await _departmentService.TSearchDepartmentsByNameAsync(name);
//            return Ok(result);
//        }

//        [HttpGet("{departmentId}/courses")]
//        public async Task<IActionResult> GetCoursesByDepartmentId(int departmentId)
//        {
//            var result = await _departmentService.TGetCoursesByDepartmentIdAsync(departmentId);
//            return Ok(result);
//        }

//        [HttpGet("get-details-by-id/{id}")]
//        public async Task<IActionResult> GetByIdWithDetails(int id)
//        {
//            var result = await _departmentService.TGetByIdWithDetailsAsync(id);
//            if (result == null) return NotFound();
//            return Ok(result);
//        }

//        [HttpGet("course-count/{departmentId}")]
//        public async Task<IActionResult> GetCourseCount(int departmentId)
//        {
//            var result = await _departmentService.TGetCourseCountAsync(departmentId);
//            return Ok(result);
//        }

//        [HttpGet("faculty-count/{departmentId}")]
//        public async Task<IActionResult> GetFacultyMemberCount(int departmentId)
//        {
//            var result = await _departmentService.TGetFacultyMemberCountAsync(departmentId);
//            return Ok(result);
//        }


//    }
//}
