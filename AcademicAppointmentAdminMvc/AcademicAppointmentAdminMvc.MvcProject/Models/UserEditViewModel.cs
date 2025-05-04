using AcademicAppointmentShare.Dtos.SchoolDtos;
using AcademicAppointmentShare.Dtos.UserDtos;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AcademicAppointmentAdminMvc.MvcProject.Models { 
    public class UserEditViewModel
{
    public UpdateUserDto User { get; set; }
        [ValidateNever]

        public List<SchoolDetailDto> SchoolsWithDepartments { get; set; }
}
}
