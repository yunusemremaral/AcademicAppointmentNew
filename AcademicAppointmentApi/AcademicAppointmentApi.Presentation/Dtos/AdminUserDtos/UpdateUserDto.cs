namespace AcademicAppointmentApi.Presentation.Dtos.AdminUserDtos
{
    public class UpdateUserDto
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public int? SchoolId { get; set; }
        public int? DepartmentId { get; set; }
        public int? RoomId { get; set; }
    }
}
