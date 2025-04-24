using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.EntityLayer.Entities
{
    public class Room
    {
        public string Id { get; set; }  // Odanın benzersiz ID'si
        public string Name { get; set; }  // Oda adı

        // Her odanın kesinlikle bir kullanıcısı olacak
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }  // Odaya atanmış kullanıcı
    }


}
