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
    public class EfSchoolRepository : GenericRepository<School>, ISchoolRepository
    {
        private readonly Context _context;

        public EfSchoolRepository(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<List<School>> GetAllWithDepartmentsAsync()
        {
            return await _context.Schools
                .Include(s => s.Departments)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Department>> GetDepartmentsBySchoolIdAsync(int schoolId)
        {
            var school = await _context.Schools
                .Include(s => s.Departments)
                .FirstOrDefaultAsync(s => s.Id == schoolId);

            return school?.Departments?.ToList() ?? new List<Department>();
        }
        public async Task<int> GetDepartmentCountAsync(int schoolId)
        {
            return await _context.Departments.CountAsync(d => d.SchoolId == schoolId);
        }
    }
}

