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
    public class SchoolService : GenericService<School>, ISchoolService
    {
        public SchoolService(ISchoolRepository schoolRepository) : base(schoolRepository)
        {
        }
    }
}
