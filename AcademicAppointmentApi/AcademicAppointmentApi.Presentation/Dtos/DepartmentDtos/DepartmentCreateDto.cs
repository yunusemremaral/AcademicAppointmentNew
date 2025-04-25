namespace AcademicAppointmentApi.Presentation.Dtos.DepartmentDtos
{
    public class DepartmentCreateDto
    {
        // Bölüm adı
        public string Name { get; set; }

        // Hangi okula ait
        public string SchoolId { get; set; }
    }
}
