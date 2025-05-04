using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentShare.Dtos.UserDtos
{
    public class UpdateUserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public string Email { get; set; }
        public int? SchoolId { get; set; }
        public int? DepartmentId { get; set; }
    }
}
