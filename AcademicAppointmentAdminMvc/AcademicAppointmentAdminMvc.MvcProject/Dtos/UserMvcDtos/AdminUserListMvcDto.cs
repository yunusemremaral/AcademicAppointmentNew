namespace AcademicAppointmentAdminMvc.MvcProject.Dtos.UserMvcDtos
{
    public class AdminUserListMvcDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? SchoolName { get; set; }
        public string? DepartmentName { get; set; }
        public List<string> Roles { get; set; }

    }
}
