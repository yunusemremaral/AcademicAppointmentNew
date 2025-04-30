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

        public SchoolListDto School { get; set; }
        public List<CourseDepartmentDto> Courses { get; set; }
    }
}
