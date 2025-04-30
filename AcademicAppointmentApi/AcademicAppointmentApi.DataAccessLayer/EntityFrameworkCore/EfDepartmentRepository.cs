using AcademicAppointmentApi.DataAccessLayer.Abstract;
using AcademicAppointmentApi.DataAccessLayer.Concrete;
using AcademicAppointmentApi.DataAccessLayer.EntityFrameworkCore;
using AcademicAppointmentApi.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.DataAccessLayer.EntityFrameworkCore
{
    public class EfDepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        private readonly Context _context;

        public EfDepartmentRepository(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Department>> GetAllWithCoursesAsync()
        {
                return await _context.Departments
        .Include(d => d.Courses)
        .Include(d => d.School) // School'u ekledik
        .AsNoTracking()
        .ToListAsync();

        }

       

        

        public async Task<IReadOnlyList<Department>> GetDepartmentsBySchoolIdAsync(int schoolId)
        {
            return await _context.Departments
                .Where(d => d.SchoolId == schoolId)
                .ToListAsync();
        }



        public async Task<IReadOnlyList<Course>> GetCoursesByDepartmentIdAsync(int departmentId)
        {
            return await _context.Courses
                .Where(c => c.DepartmentId == departmentId)
                .Include(c => c.Department) // Department ile olan ilişkiyi dahil et
                .Include(c => c.Instructor) // Instructor ile olan ilişkiyi dahil et
                .ToListAsync();
        }

        public async Task<Department?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Departments
                .Include(d => d.School)
                .Include(d => d.Courses)
                .Include(d => d.FacultyMembers)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<int> GetCourseCountAsync(int departmentId)
        {
            return await _context.Courses
                .CountAsync(c => c.DepartmentId == departmentId);
        }

    
    }
}
