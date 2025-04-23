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

        // Foreign key to the academic user
        public string AcademicUserId { get; set; }
        public virtual AppUser AcademicUser { get; set; }

        // Foreign key to the student user
        public string StudentUserId { get; set; }
        public virtual AppUser StudentUser { get; set; }

        // Appointment details
        public DateTime ScheduledAt { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public AppointmentStatus Status { get; set; }

    }
    public enum AppointmentStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Completed
    }
}
