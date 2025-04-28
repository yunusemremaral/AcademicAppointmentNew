namespace AcademicAppointmentApi.Presentation.Dtos.AdminUserDtos
{
    public class AdminUserDetailDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int? SchoolId { get; set; }
        public int? DepartmentId { get; set; }
        public int? RoomId { get; set; }
        public string? SchoolName { get; set; }
        public string? DepartmentName { get; set; }

    }
}
