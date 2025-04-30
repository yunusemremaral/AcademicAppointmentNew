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
    public class EfCourseRepository : GenericRepository<Course>, ICourseRepository
    {
        private readonly Context _context;

        public EfCourseRepository(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<Course?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Courses
                .Where(c => c.Id == id)
                .Select(c => new Course
                {
                    Id = c.Id,
                    Name = c.Name,
                    DepartmentId = c.DepartmentId,
                    Department = new Department
                    {
                        Id = c.Department.Id,
                        Name = c.Department.Name,
                        SchoolId = c.Department.SchoolId,
                        School = new School
                        {
                            Id = c.Department.School.Id,
                            Name = c.Department.School.Name
                        }
                    },
                    InstructorId = c.InstructorId,
                    Instructor = new AppUser
                    {
                        Id = c.Instructor.Id,
                        UserName = c.Instructor.UserName,
                        Email = c.Instructor.Email
                    }
                })
                .FirstOrDefaultAsync();
        }

        public async Task<List<Course>> GetAllWithDetailsAsync()
        {
            return await _context.Courses
                .Select(c => new Course
                {
                    Id = c.Id,
                    Name = c.Name,
                    DepartmentId = c.DepartmentId,
                    Department = new Department
                    {
                        Id = c.Department.Id,
                        Name = c.Department.Name,
                        SchoolId = c.Department.SchoolId,
                        School = new School
                        {
                            Id = c.Department.School.Id,
                            Name = c.Department.School.Name
                        }
                    },
                    InstructorId = c.InstructorId,
                    Instructor = new AppUser
                    {
                        Id = c.Instructor.Id,
                        UserName = c.Instructor.UserName,
                        Email = c.Instructor.Email
                    }
                })
                .ToListAsync();
        }

        public async Task<List<Course>> GetAllByInstructorIdWithDetailsAsync(string instructorId)
        {
            return await _context.Courses
                .Where(c => c.InstructorId == instructorId)
                .Select(c => new Course
                {
                    Id = c.Id,
                    Name = c.Name,
                    DepartmentId = c.DepartmentId,
                    Department = new Department
                    {
                        Id = c.Department.Id,
                        Name = c.Department.Name,
                        SchoolId = c.Department.SchoolId,
                        School = new School
                        {
                            Id = c.Department.School.Id,
                            Name = c.Department.School.Name
                        }
                    },
                    InstructorId = c.InstructorId,
                    Instructor = new AppUser
                    {
                        Id = c.Instructor.Id,
                        UserName = c.Instructor.UserName,
                        Email = c.Instructor.Email
                    }
                })
                .ToListAsync();
        }
    }
}
