using AcademicAppointmentApi.DataAccessLayer.Abstract;
using AcademicAppointmentApi.DataAccessLayer.Concrete;
using AcademicAppointmentApi.DataAccessLayer.EntityFrameworkCore;
using AcademicAppointmentApi.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.DataAccessLayer.Repositories
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
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Department>> GetAllWithFacultyMembersAsync()
        {
            var instructorIds = await _context.UserRoles
                .Where(ur => _context.Roles
                    .Where(r => r.Name == "Instructor")
                    .Select(r => r.Id)
                    .Contains(ur.RoleId))
                .Select(ur => ur.UserId)
                .ToListAsync();

            return await _context.Departments
                .Include(d => d.FacultyMembers.Where(u => instructorIds.Contains(u.Id)))
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Department>> GetAllWithStudentsAsync()
        {
            var studentIds = await _context.UserRoles
                .Where(ur => _context.Roles
                    .Where(r => r.Name == "Student")
                    .Select(r => r.Id)
                    .Contains(ur.RoleId))
                .Select(ur => ur.UserId)
                .ToListAsync();

            return await _context.Departments
                .Include(d => d.FacultyMembers.Where(u => studentIds.Contains(u.Id)))
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Department>> GetDepartmentsBySchoolIdAsync(int schoolId)
        {
            return await _context.Departments
                .Where(d => d.SchoolId == schoolId)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Department>> SearchDepartmentsByNameAsync(string name)
        {
            return await _context.Departments
                .Where(d => d.Name.Contains(name))
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Course>> GetCoursesByDepartmentIdAsync(int departmentId)
        {
            return await _context.Courses
                .Where(c => c.DepartmentId == departmentId)
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

        public async Task<int> GetFacultyMemberCountAsync(int departmentId)
        {
            var instructorIds = await _context.UserRoles
                .Where(ur => _context.Roles
                    .Where(r => r.Name == "Instructor")
                    .Select(r => r.Id)
                    .Contains(ur.RoleId))
                .Select(ur => ur.UserId)
                .ToListAsync();

            return await _context.Users
                .CountAsync(u => u.DepartmentId == departmentId && instructorIds.Contains(u.Id));
        }
    }
}
