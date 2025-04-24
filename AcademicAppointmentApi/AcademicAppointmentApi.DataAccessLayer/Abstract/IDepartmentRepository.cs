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
        Task<List<Department>> GetDepartmentsBySchoolIdAsync(string schoolId);
    }
}
