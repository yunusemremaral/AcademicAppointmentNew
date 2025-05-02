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





        
        public async Task<IReadOnlyList<Course>> GetCoursesByDepartmentIdAsync(int departmentId)
        {
            return await _context.Courses
                .Where(c => c.DepartmentId == departmentId)
                .Select(c => new Course
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }
        public async Task<IReadOnlyList<Course>> GetCoursesWithInstructorByDepartmentIdAsync(int departmentId)
        {
            return await _context.Courses
                .Where(c => c.DepartmentId == departmentId)
                .Select(c => new Course
                {
                    Id = c.Id,
                    Name = c.Name,
                    InstructorId = c.InstructorId,
                    Instructor = new AppUser
                    {
                       Id = c.Instructor.Id,
                       UserName = c.Instructor.UserName,
                       Email = c.Instructor.Email
                    },

                })
                .AsNoTracking()
                .ToListAsync();
        }



        public async Task<Department?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Departments
                .Where(d => d.Id == id)
                .Select(d => new Department
                {
                    Id = d.Id,
                    Name = d.Name,
                    SchoolId = d.SchoolId,
                    School = new School
                    {
                        Id = d.School.Id,
                        Name = d.School.Name
                    },
                    FacultyMembers = d.FacultyMembers,
                    // İhtiyaç varsa eklenebilir
                    Courses = d.Courses // İhtiyaç varsa eklenebilir
                })
                .FirstOrDefaultAsync();
        }
        public async Task<IReadOnlyList<Department>> GetDepartmentsWithSchoolAsync()
        {
            return await _context.Departments
                .Select(d => new Department
                {
                    Id = d.Id,
                    Name = d.Name,
                    School = new School
                    {
                        Id = d.School.Id,
                        Name = d.School.Name
                    }
                })
                .AsNoTracking()
                .ToListAsync();
        }


        public async Task<int> GetCourseCountAsync(int departmentId)
        {
            return await _context.Courses
                .CountAsync(c => c.DepartmentId == departmentId);
        }

    
    }
}
