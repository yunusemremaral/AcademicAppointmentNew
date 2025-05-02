using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AcademicAppointmentAdminMvc.MvcProject.Models
{
    // CourseCreateViewModel.cs
    public class CourseCreateViewModel
    {
        [Required(ErrorMessage = "Ders adı zorunludur.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bölüm seçimi zorunludur.")]
        public int DepartmentId { get; set; } // Nullable yapıldı

        [Required(ErrorMessage = "Akademisyen seçimi zorunludur.")]
        public string InstructorId { get; set; }

        public int? SchoolId { get; set; }
        public SelectList Schools { get; set; }
        public SelectList Departments { get; set; }
        public SelectList Instructors { get; set; }
    }
}
