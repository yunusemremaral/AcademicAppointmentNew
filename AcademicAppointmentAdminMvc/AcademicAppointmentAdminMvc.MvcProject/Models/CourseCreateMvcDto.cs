using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentShare.Dtos.CourseDtos
{
    // CourseCreateMvcDto.cs
    public class CourseCreateMvcDto
    {
        [Required(ErrorMessage = "Kurs adı zorunlu")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Okul seçimi zorunlu")]
        [Display(Name = "Okul")]
        public int SchoolId { get; set; }

        [Required(ErrorMessage = "Bölüm seçimi zorunlu")]
        [Display(Name = "Bölüm")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Eğitmen seçimi zorunlu")]
        [Display(Name = "Eğitmen")]
        public string InstructorId { get; set; }

        // Dropdown listeleri
        public SelectList Schools { get; set; }
        public SelectList Departments { get; set; }
        public SelectList Instructors { get; set; }
    }
}
