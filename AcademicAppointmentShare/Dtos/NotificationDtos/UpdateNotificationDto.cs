using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentShare.Dtos.NotificationDtos
{
    public class UpdateNotificationDto
    {
        public int Id { get; set; } // Güncelleme için ID gerekli
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
    }
}
