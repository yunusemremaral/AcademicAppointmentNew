using AcademicAppointmentApi.DataAccessLayer.Abstract;
using AcademicAppointmentApi.DataAccessLayer.Concrete;
using AcademicAppointmentApi.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.DataAccessLayer.EntityFrameworkCore
{
    public class EfMessageRepository : GenericRepository<Message>, IMessageRepository
    {
        private readonly Context _context;

        public EfMessageRepository(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Message>> GetMessagesByUserIdAsync(string userId)
        {
            return await _context.Messages
                .Where(m => m.SenderId == userId || m.ReceiverId == userId)
                .OrderByDescending(m => m.SentAt)
                .Select(m => new Message
                {
                    Id = m.Id,
                    Content = m.Content,
                    SentAt = m.SentAt,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Sender = m.Sender == null ? null : new AppUser
                    {
                        Id = m.Sender.Id,
                        UserName = m.Sender.UserName,
                        Email = m.Sender.Email
                    },
                    Receiver = m.Receiver == null ? null : new AppUser
                    {
                        Id = m.Receiver.Id,
                        UserName = m.Receiver.UserName,
                        Email = m.Receiver.Email
                    }
                })
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Message>> GetConversationAsync(string userId1, string userId2)
        {
            return await _context.Messages
                .Where(m => (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                            (m.SenderId == userId2 && m.ReceiverId == userId1))
                .OrderBy(m => m.SentAt)
                .Select(m => new Message
                {
                    Id = m.Id,
                    Content = m.Content,
                    SentAt = m.SentAt,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Sender = m.Sender == null ? null : new AppUser
                    {
                        Id = m.Sender.Id,
                        UserName = m.Sender.UserName,
                        Email = m.Sender.Email
                    },
                    Receiver = m.Receiver == null ? null : new AppUser
                    {
                        Id = m.Receiver.Id,
                        UserName = m.Receiver.UserName,
                        Email = m.Receiver.Email
                    }
                })
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Message>> GetSentMessagesAsync(string senderId)
        {
            return await _context.Messages
                .Where(m => m.SenderId == senderId)
                .OrderByDescending(m => m.SentAt)
                .Select(m => new Message
                {
                    Id = m.Id,
                    Content = m.Content,
                    SentAt = m.SentAt,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Receiver = m.Receiver == null ? null : new AppUser
                    {
                        Id = m.Receiver.Id,
                        UserName = m.Receiver.UserName,
                        Email = m.Receiver.Email
                    }
                })
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Message>> GetReceivedMessagesAsync(string receiverId)
        {
            return await _context.Messages
                .Where(m => m.ReceiverId == receiverId)
                .OrderByDescending(m => m.SentAt)
                .Select(m => new Message
                {
                    Id = m.Id,
                    Content = m.Content,
                    SentAt = m.SentAt,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Sender = m.Sender == null ? null : new AppUser
                    {
                        Id = m.Sender.Id,
                        UserName = m.Sender.UserName,
                        Email = m.Sender.Email
                    }
                })
                .ToListAsync();
        }
    }
}
