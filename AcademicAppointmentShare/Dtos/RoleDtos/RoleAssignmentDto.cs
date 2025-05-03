using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentShare.Dtos.RoleDtos
{
    public class RoleAssignmentDto
    {
        public string UserId { get; set; }  // Kullanıcı ID'si
        public string RoleName { get; set; }  // Atanacak rol adı
    }
}
