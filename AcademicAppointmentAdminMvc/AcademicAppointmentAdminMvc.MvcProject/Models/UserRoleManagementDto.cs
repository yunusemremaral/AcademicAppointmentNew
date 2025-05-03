using Microsoft.AspNetCore.Mvc.Rendering;

namespace AcademicAppointmentAdminMvc.MvcProject.Models
{
    public class UserRoleManagementDto
    {
        public string UserId { get; set; }
    public string UserName { get; set; }
    public List<SelectListItem> AllRoles { get; set; } = new();
    public List<string> AssignedRoles { get; set; } = new();
    public string RoleName { get; set; }
}
}
