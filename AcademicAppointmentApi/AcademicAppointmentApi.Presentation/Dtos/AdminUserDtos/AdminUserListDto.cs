namespace AcademicAppointmentApi.Presentation.Dtos.AdminUserDtos
{
    public class AdminUserListDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? SchoolName { get; set; }
        public string? DepartmentName { get; set; }

    }
}
