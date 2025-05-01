using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentShare.Dtos.AppointmentDtoS
{
    public class AppointmentResultDto
    {
        public int Id { get; set; }

        // Kullanıcı Id'leri (özellikle işlem yapılacaksa gerekli)
        public string AcademicUserId { get; set; }
        public string StudentUserId { get; set; }

        // Kullanıcı Adları (görsellik için)
        public string AcademicUserName { get; set; }
        public string StudentUserName { get; set; }

        public DateTime ScheduledAt { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }

        public AppointmentStatusDto Status { get; set; }
    }
    


}
