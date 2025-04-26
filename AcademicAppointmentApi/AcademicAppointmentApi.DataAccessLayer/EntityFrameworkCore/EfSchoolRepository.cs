using AcademicAppointmentApi.DataAccessLayer.Abstract;
using AcademicAppointmentApi.DataAccessLayer.Concrete;
using AcademicAppointmentApi.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.DataAccessLayer.EntityFrameworkCore
{
    public class EfSchoolRepository : GenericRepository<School>, ISchoolRepository
    {
        private readonly Context _context;

        public EfSchoolRepository(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<List<School>> GetSchoolsWithDepartmentsAsync()
        {
            return await _context.Schools
                .Include(s => s.Departments)
                .ToListAsync();
        }
    }
}

