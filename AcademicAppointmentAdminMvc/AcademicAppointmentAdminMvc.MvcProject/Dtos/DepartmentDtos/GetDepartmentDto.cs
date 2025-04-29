namespace AcademicAppointmentAdminMvc.MvcProject.Dtos.DepartmentDtos
{
    public class GetDepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int SchoolId { get; set; }
        public string SchoolName { get; set; }

    }
}
