using AcademicAppointmentShare.Dtos.SchoolDtos;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AcademicAppointmentAdminMvc.MvcProject.Models
{
    public class CreateUserMvcDto
    {
        [Required]
        public string UserFullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Okul")]
        public int? SchoolId { get; set; }

        [Required]
        [Display(Name = "Bölüm")]
        public int? DepartmentId { get; set; }



        [ValidateNever]
        public List<SchoolDetailDto> SchoolsWithDepartments { get; set; }
    }

}
