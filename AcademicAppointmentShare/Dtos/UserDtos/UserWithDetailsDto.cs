using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentShare.Dtos.UserDtos
{
    public class UserWithDetailsDto
    {
        public string Id { get; set; }
        public string UserFullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? SchoolId { get; set; }
        public string SchoolName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? RoomId { get; set; }
        public string RoomName { get; set; }
    }

}
