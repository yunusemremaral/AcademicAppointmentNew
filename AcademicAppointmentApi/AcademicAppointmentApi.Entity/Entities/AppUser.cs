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
        // Kullanıcının okulu
        public string SchoolId { get; set; }
        public School School { get; set; }

        // Kullanıcının bölümü
        public string DepartmentId { get; set; }
        public Department Department { get; set; }

        // Kullanıcının dersleri
        public ICollection<Course> Courses { get; set; }

        // Mesajlar
        public ICollection<Message> Messages { get; set; }

    }
}
