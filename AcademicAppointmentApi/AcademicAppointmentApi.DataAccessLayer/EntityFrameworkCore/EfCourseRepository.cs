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

        // DepartmentId'ye göre Course'ları al
        public async Task<IReadOnlyList<Course>> GetByDepartmentIdAsync(int departmentId)
        {
            return await _context.Courses
                .Where(c => c.DepartmentId == departmentId)
                .ToListAsync();
        }

        // InstructorId'ye göre Course'ları al
        public async Task<IReadOnlyList<Course>> GetByInstructorIdAsync(string instructorId)
        {
            return await _context.Courses
                .Where(c => c.InstructorId == instructorId)
                .ToListAsync();
        }

        // Course'u detaylarıyla birlikte al
        public async Task<Course> GetCourseWithDetailsAsync(int courseId)
        {
            return await _context.Courses
                .Include(c => c.Department)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(c => c.Id == courseId);
        }


    }
}
