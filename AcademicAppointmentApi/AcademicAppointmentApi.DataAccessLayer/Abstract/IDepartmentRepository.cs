using AcademicAppointmentApi.EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.DataAccessLayer.Abstract
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        Task<IReadOnlyList<Department>> GetAllWithCoursesAsync();
        Task<IReadOnlyList<Department>> GetDepartmentsBySchoolIdAsync(int schoolId);
        Task<IReadOnlyList<Course>> GetCoursesByDepartmentIdAsync(int departmentId);
        Task<Department?> GetByIdWithDetailsAsync(int id);
        Task<int> GetCourseCountAsync(int departmentId);
    }
}
