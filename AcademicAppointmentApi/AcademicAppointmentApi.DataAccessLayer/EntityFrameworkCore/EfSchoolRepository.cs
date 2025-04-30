using AcademicAppointmentApi.DataAccessLayer.Abstract;
using AcademicAppointmentApi.DataAccessLayer.Concrete;
using AcademicAppointmentApi.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;

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
            return await _context.Departments
                .Where(d => d.SchoolId == schoolId)
                .AsNoTracking()
                .ToListAsync();
        }



        public async Task<int> GetDepartmentCountAsync(int schoolId)
        {
            return await _context.Departments.CountAsync(d => d.SchoolId == schoolId);
        }
    }
}
