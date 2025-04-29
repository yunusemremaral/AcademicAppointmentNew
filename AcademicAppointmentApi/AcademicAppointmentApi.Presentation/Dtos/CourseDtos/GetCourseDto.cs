namespace AcademicAppointmentApi.Presentation.Dtos.CourseDtos
{
    public class GetCourseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public string InstructorId { get; set; }
        public string InstructorName { get; set; }

    }
}
