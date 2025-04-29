namespace AcademicAppointmentAdminMvc.MvcProject.Dtos.CourseDtos
{
    public class UpdateCourseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public int InstructorId { get; set; }

    }
}
