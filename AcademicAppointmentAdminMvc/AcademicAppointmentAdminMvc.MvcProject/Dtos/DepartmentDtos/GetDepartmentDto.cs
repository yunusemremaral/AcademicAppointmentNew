namespace AcademicAppointmentAdminMvc.MvcProject.Dtos.DepartmentDtos
{
    public class GetDepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SchoolName { get; set; } // API'de bu alan Join ile gelmeli
        public int SchoolId { get; set; }

    }
}
