using AcademicAppointmentApi.DataAccessLayer.Abstract;
using AcademicAppointmentApi.DataAccessLayer.Concrete;
using AcademicAppointmentApi.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.DataAccessLayer.EntityFrameworkCore
{
    public class EfAppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly Context _context;

        public EfAppointmentRepository(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Appointment>> GetAllAppointmentsWithUsersAsync()
        {
            return await _context.Appointments
                .Select(a => new Appointment
                {
                    Id = a.Id,
                    ScheduledAt = a.ScheduledAt,
                    Subject = a.Subject,
                    Description = a.Description,
                    Status = a.Status,
                    AcademicUserId = a.AcademicUserId,
                    AcademicUser = new AppUser
                    {
                        Id = a.AcademicUser.Id,
                        UserName = a.AcademicUser.UserName,
                        Email = a.AcademicUser.Email
                    },
                    StudentUserId = a.StudentUserId,
                    StudentUser = new AppUser
                    {
                        Id = a.StudentUser.Id,
                        UserName = a.StudentUser.UserName,
                        Email = a.StudentUser.Email
                    }
                }).ToListAsync();
        }

        public async Task<Appointment> GetAppointmentByIdWithUsersAsync(int id)
        {
            return await _context.Appointments
                .Where(a => a.Id == id)
                .Select(a => new Appointment
                {
                    Id = a.Id,
                    ScheduledAt = a.ScheduledAt,
                    Subject = a.Subject,
                    Description = a.Description,
                    Status = a.Status,
                    AcademicUserId = a.AcademicUserId,
                    AcademicUser = new AppUser
                    {
                        Id = a.AcademicUser.Id,
                        UserName = a.AcademicUser.UserName,
                        Email = a.AcademicUser.Email
                    },
                    StudentUserId = a.StudentUserId,
                    StudentUser = new AppUser
                    {
                        Id = a.StudentUser.Id,
                        UserName = a.StudentUser.UserName,
                        Email = a.StudentUser.Email
                    }
                }).FirstOrDefaultAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsByAcademicIdAsync(string academicUserId)
        {
            return await _context.Appointments
                .Where(a => a.AcademicUserId == academicUserId)
                .Select(a => new Appointment
                {
                    Id = a.Id,
                    ScheduledAt = a.ScheduledAt,
                    Subject = a.Subject,
                    Description = a.Description,
                    Status = a.Status,
                    AcademicUserId = a.AcademicUserId,
                    AcademicUser = new AppUser
                    {
                        Id = a.AcademicUser.Id,
                        UserName = a.AcademicUser.UserName,
                        Email = a.AcademicUser.Email
                    },
                    StudentUserId = a.StudentUserId,
                    StudentUser = new AppUser
                    {
                        Id = a.StudentUser.Id,
                        UserName = a.StudentUser.UserName,
                        Email = a.StudentUser.Email
                    }
                }).ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsByStudentIdAsync(string studentUserId)
        {
            return await _context.Appointments
                .Where(a => a.StudentUserId == studentUserId)
                .Select(a => new Appointment
                {
                    Id = a.Id,
                    ScheduledAt = a.ScheduledAt,
                    Subject = a.Subject,
                    Description = a.Description,
                    Status = a.Status,
                    AcademicUserId = a.AcademicUserId,
                    AcademicUser = new AppUser
                    {
                        Id = a.AcademicUser.Id,
                        UserName = a.AcademicUser.UserName,
                        Email = a.AcademicUser.Email
                    },
                    StudentUserId = a.StudentUserId,
                    StudentUser = new AppUser
                    {
                        Id = a.StudentUser.Id,
                        UserName = a.StudentUser.UserName,
                        Email = a.StudentUser.Email
                    }
                }).ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsByDateAsync(DateTime date)
        {
            return await _context.Appointments
                .Where(a => a.ScheduledAt.Date == date.Date)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsInDateRangeAsync(DateTime start, DateTime end)
        {
            return await _context.Appointments
                .Where(a => a.ScheduledAt >= start && a.ScheduledAt <= end)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetPastAppointmentsByUserIdAsync(string userId)
        {
            return await _context.Appointments
                .Where(a => (a.StudentUserId == userId || a.AcademicUserId == userId) && a.ScheduledAt < DateTime.Now)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetUpcomingAppointmentsByUserIdAsync(string userId)
        {
            return await _context.Appointments
                .Where(a => (a.StudentUserId == userId || a.AcademicUserId == userId) && a.ScheduledAt >= DateTime.Now)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsByStatusAsync(AppointmentStatus status)
        {
            return await _context.Appointments
                .Where(a => a.Status == status)
                .Select(a => new Appointment
                {
                    Id = a.Id,
                    ScheduledAt = a.ScheduledAt,
                    Subject = a.Subject,
                    Description = a.Description,
                    Status = a.Status,
                    AcademicUserId = a.AcademicUserId,
                    AcademicUser = new AppUser
                    {   
                        Id = a.AcademicUser.Id,
                        UserName = a.AcademicUser.UserName
                    },
                    StudentUserId = a.StudentUserId,
                    StudentUser = new AppUser
                    {
                        Id = a.StudentUser.Id,
                        UserName = a.StudentUser.UserName
                    }
                }).ToListAsync();
        }

    }
}
