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
    public class EfAppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        private readonly Context _context;

        public EfAppointmentRepository(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Appointment>> GetAppointmentsByStudentIdAsync(string studentId)
        {
            return await _context.Appointments
                .Where(a => a.StudentUserId == studentId)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsByAcademicIdAsync(string academicId)
        {
            return await _context.Appointments
                .Where(a => a.AcademicUserId == academicId)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetAppointmentsByDateAsync(DateTime date)
        {
            return await _context.Appointments
                .Where(a => a.Date.Date == date.Date)
                .ToListAsync();
        }
    }
}
