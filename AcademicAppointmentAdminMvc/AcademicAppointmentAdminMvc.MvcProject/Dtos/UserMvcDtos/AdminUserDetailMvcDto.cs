namespace AcademicAppointmentAdminMvc.MvcProject.Dtos.UserMvcDtos

{
    public class AdminUserDetailMvcDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int? SchoolId { get; set; }
        public int? DepartmentId { get; set; }
        public int? RoomId { get; set; }
        public string? SchoolName { get; set; }
        public string? DepartmentName { get; set; }
        public List<string> Roles { get; set; } // ✅ Yeni eklenen alan

    }
}
