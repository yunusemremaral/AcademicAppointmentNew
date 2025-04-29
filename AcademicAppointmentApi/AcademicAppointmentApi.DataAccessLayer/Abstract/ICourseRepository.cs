using AcademicAppointmentApi.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.DataAccessLayer.Abstract
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<List<Course>> GetCoursesByDepartmentIdAsync(int departmentId);
        Task<List<Course>> GetCoursesByInstructorIdAsync(string instructorId);
        Task<List<Course>> GetCoursesWithDepartmentAndInstructorAsync();
        Task<Course> GetCourseWithDetailsByIdAsync(int id);

    }
}
