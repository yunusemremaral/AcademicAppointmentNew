using AcademicAppointmentApi.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Abstract
{
    public interface IMessageService : ITGenericService<Message>
    {
        Task<List<Message>> TGetMessagesBySenderIdAsync(string senderId);
        Task<List<Message>> TGetMessagesByReceiverIdAsync(string receiverId);
    }
}
