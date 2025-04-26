using AcademicAppointmentApi.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.DataAccessLayer.Abstract
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<List<Message>> GetMessagesBySenderIdAsync(string senderId);
        Task<List<Message>> GetMessagesByReceiverIdAsync(string receiverId);
    }
}
