using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentShare.Dtos.DepartmentDtos
{
    public class CourseWithDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } // Department adı
        public string InstructorId { get; set; }
        public string InstructorName { get; set; } // Instructor adı
    }
}
