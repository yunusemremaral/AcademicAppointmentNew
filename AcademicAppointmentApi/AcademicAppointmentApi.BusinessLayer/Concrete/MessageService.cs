using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.DataAccessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Concrete
{
    public class MessageService : GenericService<Message>, IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository) : base(messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<List<Message>> GetMessagesBySenderIdAsync(string senderId)
        {
            return await _messageRepository.GetMessagesBySenderIdAsync(senderId);
        }

        public async Task<List<Message>> GetMessagesByReceiverIdAsync(string receiverId)
        {
            return await _messageRepository.GetMessagesByReceiverIdAsync(receiverId);
        }

        public async Task<List<Message>> GetConversationAsync(string user1Id, string user2Id)
        {
            return await _messageRepository.GetConversationAsync(user1Id, user2Id);
        }
    }

}
