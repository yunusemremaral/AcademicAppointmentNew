using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentShare.Dtos.CourseDtos
{
    public class CourseCreateDto
    {
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public string InstructorId { get; set; }
    }
}
