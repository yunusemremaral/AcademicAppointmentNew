using AcademicAppointmentApi.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Abstract
{
    public interface IMessageService : IGenericService<Message>
    {
        Task<List<Message>> GetMessagesBySenderIdAsync(string senderId);
        Task<List<Message>> GetMessagesByReceiverIdAsync(string receiverId);
        Task<List<Message>> GetConversationAsync(string user1Id, string user2Id);
    }
}
