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
        Task<IReadOnlyList<Message>> TGetMessagesByUserIdAsync(string userId);
        Task<IReadOnlyList<Message>> TGetConversationAsync(string userId1, string userId2);
        Task<IReadOnlyList<Message>> TGetSentMessagesAsync(string senderId);
        Task<IReadOnlyList<Message>> TGetReceivedMessagesAsync(string receiverId);
        Task<IReadOnlyList<Message>> TGetAllWithRelationsAsync();

    }
}
