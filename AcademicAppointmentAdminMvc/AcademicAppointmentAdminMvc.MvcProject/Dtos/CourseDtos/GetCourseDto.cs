namespace AcademicAppointmentAdminMvc.MvcProject.Dtos.CourseDtos
{
    public class GetCourseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public int InstructorId { get; set; }
        public string InstructorName { get; set; }

    }
}
