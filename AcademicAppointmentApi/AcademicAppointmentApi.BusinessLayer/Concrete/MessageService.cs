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
    public class MessageService : TGenericService<Message>, IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository) : base(messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<List<Message>> TGetMessagesBySenderIdAsync(string senderId)
        {
            return await _messageRepository.GetMessagesBySenderIdAsync(senderId);
        }

        public async Task<List<Message>> TGetMessagesByReceiverIdAsync(string receiverId)
        {
            return await _messageRepository.GetMessagesByReceiverIdAsync(receiverId);
        }
    }
}
