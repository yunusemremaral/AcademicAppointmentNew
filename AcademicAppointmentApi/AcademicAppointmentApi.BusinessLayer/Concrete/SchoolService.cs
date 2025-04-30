using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.DataAccessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Concrete
{
    public class SchoolService : TGenericService<School>, ISchoolService
    {
        private readonly ISchoolRepository _schoolRepository;

        public SchoolService(ISchoolRepository schoolRepository) : base(schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }

        public async Task<List<School>> TGetAllWithDepartmentsAsync()
        {
            return await _schoolRepository.GetAllWithDepartmentsAsync();
        }

        public async Task<List<Department>> TGetDepartmentsBySchoolIdAsync(int schoolId)
        {
            return await _schoolRepository.GetDepartmentsBySchoolIdAsync(schoolId);
        }

        public async Task<int> TGetDepartmentCountAsync(int schoolId)
        {
            return await _schoolRepository.GetDepartmentCountAsync(schoolId);
        }

    }
}
