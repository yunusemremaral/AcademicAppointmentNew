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
        Task<List<Appointment>> GetAllAppointmentsWithUsersAsync();
        Task<List<Appointment>> GetAppointmentsByAcademicIdAsync(string academicUserId);
        Task<List<Appointment>> GetAppointmentsByStudentIdAsync(string studentUserId);
        Task<Appointment> GetAppointmentByIdWithUsersAsync(int id);
        Task<List<Appointment>> GetAppointmentsByDateAsync(DateTime date);
        Task<List<Appointment>> GetAppointmentsInDateRangeAsync(DateTime start, DateTime end);
        Task<List<Appointment>> GetPastAppointmentsByUserIdAsync(string userId);
        Task<List<Appointment>> GetUpcomingAppointmentsByUserIdAsync(string userId);
        Task<List<Appointment>> GetAppointmentsByStatusAsync(AppointmentStatus status);
        Task<Dictionary<string, int>> GetAppointmentCountsByStatusAsync();
        Task<Dictionary<string, int>> GetDailyAppointmentCountsAsync();



    }

}
