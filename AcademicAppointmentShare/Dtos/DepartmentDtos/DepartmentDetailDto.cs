using AcademicAppointmentShare.Dtos.SchoolDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentShare.Dtos.DepartmentDtos
{
   
        public class DepartmentDetailDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string SchoolName { get; set; }
            public List<DepartmentAppUserDto> FacultyMembers { get; set; } // Öğretim üyeleri (faculty members)
            public List<DepartmentCourseDto> Courses { get; set; } // Departmandaki kurslar
        }

    
}
