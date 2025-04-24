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

        // Kullanıcıya ait oda bilgisi (nullable)
        public string? RoomId { get; set; }
        public Room Room { get; set; }  // Nullable oda

        // Mesajlar (Sender ve Receiver ilişkisi)
        public virtual ICollection<Message> MessagesSent { get; set; }  // Gönderilen mesajlar
        public virtual ICollection<Message> MessagesReceived { get; set; }  // Alınan mesajlar

        // AssignedRooms - kullanıcının atandığı odalar
        public virtual ICollection<Room> AssignedRooms { get; set; }
    }


}
