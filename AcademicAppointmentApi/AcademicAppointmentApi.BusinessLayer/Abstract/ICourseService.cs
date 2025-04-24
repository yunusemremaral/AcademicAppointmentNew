using AcademicAppointmentApi.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Abstract
{
    public interface ICourseService : IGenericService<Course>
    {
        Task<List<Course>> GetCoursesByDepartmentIdAsync(string departmentId);
        Task<List<Course>> GetCoursesByInstructorIdAsync(string instructorId);
    }
}
