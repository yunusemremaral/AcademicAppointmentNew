using AcademicAppointmentApi.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.DataAccessLayer.Abstract
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        Task<IReadOnlyList<Department>> GetAllWithCoursesAsync();
        Task<IReadOnlyList<Department>> GetAllWithFacultyMembersAsync();
        Task<IReadOnlyList<Department>> GetAllWithStudentsAsync();
        Task<IReadOnlyList<Department>> GetDepartmentsBySchoolIdAsync(int schoolId);
        Task<IReadOnlyList<Department>> SearchDepartmentsByNameAsync(string name);
        Task<IReadOnlyList<Course>> GetCoursesByDepartmentIdAsync(int departmentId);


        Task<Department?> GetByIdWithDetailsAsync(int id);
        Task<int> GetCourseCountAsync(int departmentId);
        Task<int> GetFacultyMemberCountAsync(int departmentId);
    }
}
