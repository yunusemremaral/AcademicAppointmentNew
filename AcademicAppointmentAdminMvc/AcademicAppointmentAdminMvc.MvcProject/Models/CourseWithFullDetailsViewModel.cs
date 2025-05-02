using AcademicAppointmentShare.Dtos.CourseDtos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AcademicAppointmentAdminMvc.MvcProject.Models
{
    public class CourseWithFullDetailsViewModel
    {
        public List<CourseWithFullDetailsDto> Courses { get; set; }
        public SelectList Schools { get; set; }
        public SelectList Departments { get; set; }
        public SelectList Instructors { get; set; }

        public int? SelectedSchoolId { get; set; }
        public int? SelectedDepartmentId { get; set; }
        public string SelectedInstructorId { get; set; }
    }


}
