using AcademicAppointmentApi.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Abstract
{
    public interface IAppointmentService : IGenericService<Appointment>
    {
        Task<List<Appointment>> GetAppointmentsByStudentIdAsync(string studentId);
        Task<List<Appointment>> GetAppointmentsByAcademicIdAsync(string academicId);
        Task ValidateAppointmentTime(DateTime startTime, DateTime endTime); // Özel validasyon
    }

}
