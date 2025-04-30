using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentShare.Dtos.CourseDtos
{
    public class CourseWithDetailsWithDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DepartmentName { get; set; }
        public string InstructorName { get; set; }
        public string InstructorId { get; set; }
        public string InstructorEmail { get; set; }
    }
}
