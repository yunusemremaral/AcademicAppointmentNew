using AcademicAppointmentShare.Dtos.DepartmentDtos;
using AcademicAppointmentShare.Dtos.SchoolDtos;

namespace AcademicAppointmentAdminMvc.MvcProject.Models
{
    public class DepartmentSchoolViewModel
    {
        public int? SelectedSchoolId { get; set; }
        public List<SchoolListDto> Schools { get; set; }
        public List<DepartmentWithSchoolDto> Departments { get; set; }
    }
}
