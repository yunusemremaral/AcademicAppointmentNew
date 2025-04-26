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

        public async Task<List<Course>> GetCoursesByDepartmentIdAsync(string departmentId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Course>> GetCoursesByInstructorIdAsync(string instructorId)
        {
            return await _context.Courses
                .Where(c => c.InstructorId == instructorId)
                .ToListAsync();
        }
    }

}
