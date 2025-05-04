using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentShare.Dtos.SchoolDtos;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AcademicAppointmentApi.Presentation.Controllers
{
    [Route("api/admin/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "Bearer")]
    //[Authorize(Roles = "Admin")]

        public class AdminSchoolController : ControllerBase
        {
            private readonly ISchoolService _schoolService;
            private readonly IMapper _mapper;

            public AdminSchoolController(ISchoolService schoolService, IMapper mapper)
            {
                _schoolService = schoolService;
                _mapper = mapper;
            }
            // SCHOOLU ID VE NAMELER OLARAK GETİRİYOR 
            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                var schools = await _schoolService.TGetAllAsync();
                var dtoList = _mapper.Map<List<SchoolListDto>>(schools);
                return Ok(dtoList);
            }
            //1 SCHOOLU GETİRME ID VE NAME OLARAK GETİRİR
            [HttpGet("{id}")]
            public async Task<IActionResult> GetById(int id)
            {
                var school = await _schoolService.TGetByIdAsync(id);
                if (school == null) return NotFound();
                var dto = _mapper.Map<SchoolListDto>(school);
                return Ok(dto);
            }
            //EKLEME 
            [HttpPost]
            public async Task<IActionResult> Add([FromBody] SchoolCreateDto dto)
            {
                var school = _mapper.Map<School>(dto);
                var result = await _schoolService.TAddAsync(school);
                var responseDto = _mapper.Map<SchoolListDto>(result);
                return CreatedAtAction(nameof(GetById), new { id = responseDto.Id }, responseDto);
            }
            //GÜNCELLEME
            [HttpPut("{id}")]
            public async Task<IActionResult> Update(int id, [FromBody] SchoolUpdateDto dto)
            {
                if (id != dto.Id) return BadRequest("ID uyuşmuyor.");
                var school = await _schoolService.TGetByIdAsync(id);
                if (school == null) return NotFound();
                _mapper.Map(dto, school);
                await _schoolService.TUpdateAsync(school);
                return NoContent();
            }
        // SİLME 
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var school = await _schoolService.TGetByIdAsync(id);
            if (school == null)
                return NotFound();

            // Departments doluysa silme
            if (school.Departments == null)
                return BadRequest("Bu okulun bağlı bölümleri olduğu için silinemez.");

            await _schoolService.TDeleteAsync(school);
            return NoContent();
        }

        // OKULLARIN VE DEPARTMANLARIDA BİRLİKTE DÖNÜYOR 
        [HttpGet("with-departments")]
            public async Task<IActionResult> GetAllWithDepartments()
            {
                var schools = await _schoolService.TGetAllWithDepartmentsAsync();

                var dtoList = schools.Select(s => new SchoolDetailDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Departments = s.Departments?.Select(d => _mapper.Map<SDepartmentSchoolDto>(d)).ToList() ?? new List<SDepartmentSchoolDto>()
                }).ToList();

                return Ok(dtoList);
            }

            //OKULUN ID SI İLE EŞLEŞEN BÜTÜN DEPARTMANLARI DÖNÜYOR 
            [HttpGet("{schoolId}/departments")]
            public async Task<IActionResult> GetDepartmentsBySchoolId(int schoolId)
            {
                var departments = await _schoolService.TGetDepartmentsBySchoolIdAsync(schoolId);
                var dtoList = _mapper.Map<List<SDepartmentSchoolDto>>(departments);
                return Ok(dtoList);
            }

            //OKULUN ID SINE GÖRE O OKULDAKİ BULUNAN BÖLÜM SAYISINI DÖNÜYOR 
            [HttpGet("{schoolId}/department-count")] 
            public async Task<IActionResult> GetDepartmentCount(int schoolId)
            {
                var count = await _schoolService.TGetDepartmentCountAsync(schoolId);
                return Ok(new { SchoolId = schoolId, DepartmentCount = count });
            }
        // OKULUN ID'Sİ İLE O OKULUN DETAYLARI VE DEPARTMANLARINI GETİRİR
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetSchoolDetailsWithDepartments(int id)
        {
            // Okul bilgilerini ve departmanlarını alıyoruz
            var school = await _schoolService.TGetSchoolDetailsWithDepartmentsAsync(id);

            // Eğer okul bulunamazsa 404 döndür
            if (school == null) return NotFound();

            // Okul bilgilerini ve departmanları DTO'ya dönüştürüyoruz
            var schoolDetailDto = new SchoolDetailDto
            {
                Id = school.Id,
                Name = school.Name,
                Departments = school.Departments?.Select(d => _mapper.Map<SDepartmentSchoolDto>(d)).ToList() ?? new List<SDepartmentSchoolDto>()
            };

            // Detaylı okul bilgisini döndürüyoruz
            return Ok(schoolDetailDto);
        }
        [HttpGet("count")]
        public async Task<IActionResult> GetSchoolCount()
        {
            var schools = await _schoolService.TGetAllAsync();
            return Ok(new { Count = schools.Count });
        }



    }
}

