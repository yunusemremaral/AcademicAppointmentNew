using AcademicAppointmentApi.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.DataAccessLayer.Abstract
{
    public interface IAppointmentRepository : IGenericRepository<Appointment>
    {
        Task<List<Appointment>> GetAppointmentsByStudentIdAsync(string studentId);
        Task<List<Appointment>> GetAppointmentsByAcademicIdAsync(string academicId);
        Task<List<Appointment>> GetAppointmentsByDateAsync(DateTime date);
    }

}
