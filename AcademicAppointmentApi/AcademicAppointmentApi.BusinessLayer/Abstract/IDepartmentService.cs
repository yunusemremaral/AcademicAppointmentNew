using AcademicAppointmentApi.EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Abstract
{
    public interface IDepartmentService : ITGenericService<Department>
    {
        Task<IReadOnlyList<Department>> TGetAllWithCoursesAsync();
        Task<IReadOnlyList<Course>> TGetCoursesByDepartmentIdAsync(int departmentId);
        Task<Department?> TGetByIdWithDetailsAsync(int id);
        Task<int> TGetCourseCountAsync(int departmentId);
    }
}
