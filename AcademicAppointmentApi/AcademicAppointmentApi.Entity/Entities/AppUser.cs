using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.EntityLayer.Entities
{
    public class AppUser : IdentityUser
    {
        public ICollection<Appointment> AppointmentsAsAcademic { get; set; }
        public ICollection<Appointment> AppointmentsAsStudent { get; set; }

        public int? SchoolId { get; set; }
        public School? School { get; set; }

        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public ICollection<Course> Courses { get; set; }

        public int? RoomId { get; set; } 
        public Room? Room { get; set; }

        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }


    }


}
