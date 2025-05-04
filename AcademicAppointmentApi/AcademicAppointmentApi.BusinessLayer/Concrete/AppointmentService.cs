using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.DataAccessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Concrete
{
    public class AppointmentService : TGenericService<Appointment>, IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository) : base(appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<List<Appointment>> TGetAllAppointmentsWithUsersAsync()
        {
            return await _appointmentRepository.GetAllAppointmentsWithUsersAsync();
        }

        public async Task<List<Appointment>> TGetAppointmentsByAcademicIdAsync(string academicUserId)
        {
            return await _appointmentRepository.GetAppointmentsByAcademicIdAsync(academicUserId);
        }

        public async Task<List<Appointment>> TGetAppointmentsByStudentIdAsync(string studentUserId)
        {
            return await _appointmentRepository.GetAppointmentsByStudentIdAsync(studentUserId);
        }

        public async Task<Appointment> TGetAppointmentByIdWithUsersAsync(int id)
        {
            return await _appointmentRepository.GetAppointmentByIdWithUsersAsync(id);
        }

        public async Task<List<Appointment>> TGetAppointmentsByDateAsync(DateTime date)
        {
            return await _appointmentRepository.GetAppointmentsByDateAsync(date);
        }

        public async Task<List<Appointment>> TGetAppointmentsInDateRangeAsync(DateTime start, DateTime end)
        {
            return await _appointmentRepository.GetAppointmentsInDateRangeAsync(start, end);
        }

        public async Task<List<Appointment>> TGetPastAppointmentsByUserIdAsync(string userId)
        {
            return await _appointmentRepository.GetPastAppointmentsByUserIdAsync(userId);
        }

        public async Task<List<Appointment>> TGetUpcomingAppointmentsByUserIdAsync(string userId)
        {
            return await _appointmentRepository.GetUpcomingAppointmentsByUserIdAsync(userId);
        }

        public async Task<List<Appointment>> TGetAppointmentsByStatusAsync(AppointmentStatus status)
        {
            return await _appointmentRepository.GetAppointmentsByStatusAsync(status);
        }

        public async Task<Dictionary<string, int>> TGetAppointmentCountsByStatusAsync()
        {
            return await _appointmentRepository.GetAppointmentCountsByStatusAsync();
        }

        public async Task<Dictionary<string, int>> TGetDailyAppointmentCountsAsync()
        {
            return await _appointmentRepository.GetDailyAppointmentCountsAsync();
        }
    }
}
