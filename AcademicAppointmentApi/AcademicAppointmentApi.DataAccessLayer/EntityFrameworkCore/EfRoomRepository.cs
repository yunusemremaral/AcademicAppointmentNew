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

        public async Task<List<Room>> GetAllWithUsersAsync()
        {
            return await _context.Rooms
                .Select(r => new Room
                {
                    Id = r.Id,
                    Name = r.Name,
                    AppUserId = r.AppUserId,
                    AppUser = r.AppUser == null ? null : new AppUser
                    {
                        Id = r.AppUser.Id,
                        UserName = r.AppUser.UserName,
                        Email = r.AppUser.Email
                    }
                })
                .ToListAsync();
        }

        public async Task<Room?> GetByIdWithUserAsync(int id)
        {
            return await _context.Rooms
                .Where(r => r.Id == id)
                .Select(r => new Room
                {
                    Id = r.Id,
                    Name = r.Name,
                    AppUserId = r.AppUserId,
                    AppUser = r.AppUser == null ? null : new AppUser
                    {
                        Id = r.AppUser.Id,
                        UserName = r.AppUser.UserName,
                        Email = r.AppUser.Email
                    }
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Room?> GetByUserIdAsync(string userId)
        {
            return await _context.Rooms
                .Where(r => r.AppUserId == userId)
                .Select(r => new Room
                {
                    Id = r.Id,
                    Name = r.Name,
                    AppUserId = r.AppUserId,
                    AppUser = r.AppUser == null ? null : new AppUser
                    {
                        Id = r.AppUser.Id,
                        UserName = r.AppUser.UserName,
                        Email = r.AppUser.Email
                    }
                })
                .FirstOrDefaultAsync();
        }

    }
}
