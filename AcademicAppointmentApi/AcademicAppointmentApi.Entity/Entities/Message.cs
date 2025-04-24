using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.EntityLayer.Entities
{
    public class Message
    {
        public string Id { get; set; }  // Mesajın benzersiz ID'si
        public string SenderId { get; set; }  // Gönderen kullanıcı
        public AppUser Sender { get; set; }  // Gönderen kullanıcı
        public string ReceiverId { get; set; }  // Alıcı kullanıcı
        public AppUser Receiver { get; set; }  // Alıcı kullanıcı
        public string Content { get; set; }  // Mesaj içeriği
        public DateTime SentAt { get; set; }  // Mesaj gönderilme tarihi
    }


}
