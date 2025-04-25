namespace AcademicAppointmentApi.Presentation.Dtos.DepartmentDtos
{
    public class DepartmentUpdateDto
    {
        // Bölüm adı (güncelleme imkânı)
        public string Name { get; set; }

        // İsteğe bağlı: okulu değiştirmek isterseniz
        public string SchoolId { get; set; }
    }
}
