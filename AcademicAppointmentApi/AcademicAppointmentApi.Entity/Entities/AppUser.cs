using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.EntityLayer.Entities
{
    public  class AppUser : IdentityUser 
    {
        // Akademisyen olarak katıldığı randevular
        public virtual ICollection<Appointment> AppointmentsAsAcademic { get; set; }

        // Öğrenci olarak katıldığı randevular
        public virtual ICollection<Appointment> AppointmentsAsStudent { get; set; }

    }
}
