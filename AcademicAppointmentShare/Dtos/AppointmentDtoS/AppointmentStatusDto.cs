using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentShare.Dtos.AppointmentDtoS
{
    public enum AppointmentStatusDto
    {
        Pending = 0,    // Pending status
        Confirmed = 1,  // Confirmed status
        Cancelled = 2,  // Cancelled status
        Completed = 3   // Completed status
    }
}
