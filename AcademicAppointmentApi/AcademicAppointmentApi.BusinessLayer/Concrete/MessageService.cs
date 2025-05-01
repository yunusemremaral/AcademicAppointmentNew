using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.DataAccessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Concrete
{
    public class MessageService : TGenericService<Message>, IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository) : base(messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<IReadOnlyList<Message>> TGetMessagesByUserIdAsync(string userId)
        {
            return await _messageRepository.GetMessagesByUserIdAsync(userId);
        }

        public async Task<IReadOnlyList<Message>> TGetConversationAsync(string userId1, string userId2)
        {
            return await _messageRepository.GetConversationAsync(userId1, userId2);
        }

        public async Task<IReadOnlyList<Message>> TGetSentMessagesAsync(string senderId)
        {
            return await _messageRepository.GetSentMessagesAsync(senderId);
        }

        public async Task<IReadOnlyList<Message>> TGetReceivedMessagesAsync(string receiverId)
        {
            return await _messageRepository.GetReceivedMessagesAsync(receiverId);
        }
        public async Task<IReadOnlyList<Message>> TGetAllWithRelationsAsync()
        {
            return await _messageRepository.GetAllWithRelationsAsync();
        }
    }
}
