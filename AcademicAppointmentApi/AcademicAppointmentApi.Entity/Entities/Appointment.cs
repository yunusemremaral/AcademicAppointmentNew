using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.EntityLayer.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public string AcademicUserId { get; set; }
        public AppUser AcademicUser { get; set; }
        public string StudentUserId { get; set; }
        public AppUser StudentUser { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public AppointmentStatus Status { get; set; }
    }

    public enum AppointmentStatus
    {
        Pending = 0,    // Pending status
        Confirmed = 1,  // Confirmed status
        Cancelled = 2,  // Cancelled status
        Completed = 3   // Completed status
    }

}
