using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentShare.Dtos.CourseDtos
{
    public class CourseWithInstructorAndDepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string DepartmentName { get; set; }
        public string SchoolName { get; set; }

        public string InstructorUserName { get; set; }
        public string InstructorEmail { get; set; }
    }
}
