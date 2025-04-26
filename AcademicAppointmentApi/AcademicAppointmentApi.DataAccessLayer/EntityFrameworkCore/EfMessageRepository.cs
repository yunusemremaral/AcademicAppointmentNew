using AcademicAppointmentApi.DataAccessLayer.Abstract;
using AcademicAppointmentApi.DataAccessLayer.Concrete;
using AcademicAppointmentApi.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<List<Message>> GetMessagesBySenderIdAsync(string senderId)
        {
            return await _context.Messages
                .Where(m => m.SenderId == senderId)
                .ToListAsync();
        }

        public async Task<List<Message>> GetMessagesByReceiverIdAsync(string receiverId)
        {
            return await _context.Messages
                .Where(m => m.ReceiverId == receiverId)
                .ToListAsync();
        }
    }
}
