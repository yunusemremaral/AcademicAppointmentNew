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

        public async Task<School> GetSchoolDetailsWithDepartmentsAsync(int id)
        {
            return await _context.Schools
                .Include(s => s.Departments)  // Departmanları dahil et
                .AsNoTracking()  // Takip etme, sadece okuma işlemi yapacağımız için performans sağlar
                .FirstOrDefaultAsync(s => s.Id == id);  // Belirtilen okul id'sini bul
        }


        public async Task<int> GetDepartmentCountAsync(int schoolId)
        {
            return await _context.Departments.CountAsync(d => d.SchoolId == schoolId);
        }
        public async Task<List<School>> GetLatest5SchoolsAsync()
        {
            return await _context.Schools
                .OrderByDescending(s => s.Id) // GUID sırasına göre ters sıralama
                .Take(5) // Son 5 okulu al
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
