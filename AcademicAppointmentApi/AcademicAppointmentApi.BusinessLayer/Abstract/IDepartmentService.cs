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
        Task<List<Department>> TGetDepartmentsBySchoolIdAsync(int schoolId);
        Task<List<Department>> TGetDepartmentsWithSchoolAsync();
        Task<List<Course>> TGetCoursesByDepartmentIdAsync(int departmentId);


    }

}
