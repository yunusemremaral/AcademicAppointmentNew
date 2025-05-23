﻿using AcademicAppointmentApi.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.DataAccessLayer.Abstract
{
    public interface ISchoolRepository : IGenericRepository<School>
    {
        Task<List<School>> GetAllWithDepartmentsAsync();
        Task<List<Department>> GetDepartmentsBySchoolIdAsync(int schoolId);
        Task<int> GetDepartmentCountAsync(int schoolId);
        Task<School> GetSchoolDetailsWithDepartmentsAsync(int schoolId);
        Task<List<School>> GetLatest5SchoolsAsync();

    }
}
