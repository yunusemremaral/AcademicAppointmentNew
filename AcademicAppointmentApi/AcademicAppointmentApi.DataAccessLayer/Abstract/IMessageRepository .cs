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
        Task<IReadOnlyList<Message>> GetMessagesByUserIdAsync(string userId);
        Task<IReadOnlyList<Message>> GetConversationAsync(string userId1, string userId2);
        Task<IReadOnlyList<Message>> GetSentMessagesAsync(string senderId);
        Task<IReadOnlyList<Message>> GetReceivedMessagesAsync(string receiverId);
    }
}
