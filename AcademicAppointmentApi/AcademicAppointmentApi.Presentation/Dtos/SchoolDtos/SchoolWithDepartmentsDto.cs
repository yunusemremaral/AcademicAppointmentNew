namespace AcademicAppointmentApi.Presentation.Dtos.SchoolDtos
{
    public class SchoolWithDepartmentsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<DepartmentDto> Departments { get; set; }

    }
}
