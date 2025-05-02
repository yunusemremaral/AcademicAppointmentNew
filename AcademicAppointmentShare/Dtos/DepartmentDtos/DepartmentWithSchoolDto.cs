using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentShare.Dtos.DepartmentDtos
{
    public class DepartmentWithSchoolDto
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public int SchoolId { get; set; }
        public string SchoolName { get; set; }
    }
}
