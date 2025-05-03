using Microsoft.AspNetCore.Mvc.Rendering;

namespace AcademicAppointmentAdminMvc.MvcProject.Models
{
    public class CourseUpdateMvcDto
    {
        public int Id { get; set; } // ← EKLENDİ

        public string Name { get; set; }
        public int SchoolId { get; set; }
        public int DepartmentId { get; set; }
        public string InstructorId { get; set; }

        public SelectList Schools { get; set; }
        public SelectList Departments { get; set; }
        public SelectList Instructors { get; set; }
    }
}
