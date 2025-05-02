using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentShare.Dtos.DepartmentDtos
{
    public class DepartmentCourseWithInstructorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string InstructorId { get; set; }
        public string InstructorUserName { get; set; }
        public string InstructorEmail { get; set; }
    }

}
