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
        Task<IReadOnlyList<Course>> GetByDepartmentIdAsync(int departmentId);
        Task<IReadOnlyList<Course>> GetByInstructorIdAsync(string instructorId);
        Task<Course> GetCourseWithDetailsAsync(int courseId);

    }
}
