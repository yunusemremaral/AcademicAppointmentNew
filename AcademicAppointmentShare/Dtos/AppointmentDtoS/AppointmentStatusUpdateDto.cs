using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentShare.Dtos.AppointmentDtoS
{
    public class AppointmentStatusUpdateDto
    {
        public int AppointmentId { get; set; }
        public int Status { get; set; } // Enum değeri int olarak gelir
    }
}
