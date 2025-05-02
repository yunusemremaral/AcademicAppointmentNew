using AcademicAppointmentShare.Dtos.DepartmentDtos;

namespace AcademicAppointmentAdminMvc.MvcProject.Models
{
    public class DepartmentCoursesViewModel
    {
        public DepartmentDetailDto Department { get; set; }
        public List<DepartmentCourseDto> Courses { get; set; }
    }
}
