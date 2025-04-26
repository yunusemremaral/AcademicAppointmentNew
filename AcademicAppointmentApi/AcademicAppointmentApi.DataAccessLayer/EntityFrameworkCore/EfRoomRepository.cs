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
    public class EfRoomRepository : GenericRepository<Room>, IRoomRepository
    {
        private readonly Context _context;

        public EfRoomRepository(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<Room?> GetRoomByUserIdAsync(string userId)
        {
            return await _context.Rooms
                .FirstOrDefaultAsync(r => r.AppUserId == userId);
        }
    }
}
