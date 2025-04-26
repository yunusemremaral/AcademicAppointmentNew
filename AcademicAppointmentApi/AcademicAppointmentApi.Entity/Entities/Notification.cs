using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.EntityLayer.Entities
{
    public class Notification
    {

        public int Id { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }


}
