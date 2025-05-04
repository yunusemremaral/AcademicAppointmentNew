using AcademicAppointmentApi.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Abstract
{
    public interface IAppointmentService : ITGenericService<Appointment>
    {
        Task<List<Appointment>> TGetAllAppointmentsWithUsersAsync();
        Task<List<Appointment>> TGetAppointmentsByAcademicIdAsync(string academicUserId);
        Task<List<Appointment>> TGetAppointmentsByStudentIdAsync(string studentUserId);
        Task<Appointment> TGetAppointmentByIdWithUsersAsync(int id);
        Task<List<Appointment>> TGetAppointmentsByDateAsync(DateTime date);
        Task<List<Appointment>> TGetAppointmentsInDateRangeAsync(DateTime start, DateTime end);
        Task<List<Appointment>> TGetPastAppointmentsByUserIdAsync(string userId);
        Task<List<Appointment>> TGetUpcomingAppointmentsByUserIdAsync(string userId);
        Task<List<Appointment>> TGetAppointmentsByStatusAsync(AppointmentStatus status);
        Task<Dictionary<string, int>> TGetAppointmentCountsByStatusAsync();

    }
}
