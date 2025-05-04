using AcademicAppointmentApi.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Abstract
{
    public interface ISchoolService : ITGenericService<School>
    {
        Task<List<School>> TGetAllWithDepartmentsAsync();
        Task<List<Department>> TGetDepartmentsBySchoolIdAsync(int schoolId);
        Task<int> TGetDepartmentCountAsync(int schoolId);
        Task<School> TGetSchoolDetailsWithDepartmentsAsync(int schoolId);

        Task<List<School>> TGetLatest5SchoolsAsync();


    }

}
