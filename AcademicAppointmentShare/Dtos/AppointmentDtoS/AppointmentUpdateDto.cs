using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentShare.Dtos.AppointmentDtoS
{
    public class AppointmentUpdateDto
    {
        public int Id { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public int Status { get; set; } // int gelir
    }
}
