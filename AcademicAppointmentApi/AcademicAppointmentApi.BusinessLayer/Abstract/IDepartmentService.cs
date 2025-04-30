using AcademicAppointmentApi.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Abstract
{
    public interface IDepartmentService : ITGenericService<Department>
    {
        Task<IReadOnlyList<Department>> TGetAllWithCoursesAsync();
        Task<IReadOnlyList<Department>> TGetAllWithFacultyMembersAsync();
        Task<IReadOnlyList<Department>> TGetAllWithStudentsAsync();
        Task<IReadOnlyList<Department>> TGetDepartmentsBySchoolIdAsync(int schoolId);
        Task<IReadOnlyList<Department>> TSearchDepartmentsByNameAsync(string name);
        Task<IReadOnlyList<Course>> TGetCoursesByDepartmentIdAsync(int departmentId);
        Task<Department?> TGetByIdWithDetailsAsync(int id);
        Task<int> TGetCourseCountAsync(int departmentId);
        Task<int> TGetFacultyMemberCountAsync(int departmentId);


    }

}
