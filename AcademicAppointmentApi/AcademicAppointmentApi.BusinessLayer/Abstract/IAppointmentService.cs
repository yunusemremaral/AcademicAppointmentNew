using AcademicAppointmentApi.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Abstract
{
    public interface IAppointmentService : ITGenericService<Appointment>
    {
        Task<List<Appointment>> TGetAppointmentsByStudentIdAsync(string studentId);
        Task<List<Appointment>> TGetAppointmentsByAcademicIdAsync(string academicId);
    }


}
