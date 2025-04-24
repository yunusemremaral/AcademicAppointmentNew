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
    public class AppointmentService : GenericService<Appointment>, IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository) : base(appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<List<Appointment>> GetAppointmentsByStudentIdAsync(string studentId)
        {
            // İş mantığı eklenebilir (örneğin, öğrencinin aktif olup olmadığı kontrolü)
            return await _appointmentRepository.GetAppointmentsByStudentIdAsync(studentId);
        }

        public async Task<List<Appointment>> GetAppointmentsByAcademicIdAsync(string academicId)
        {
            // Akademisyenin müsaitlik durumu kontrolü
            return await _appointmentRepository.GetAppointmentsByAcademicIdAsync(academicId);
        }

        public async Task ValidateAppointmentTime(DateTime startTime, DateTime endTime)
        {
            if (startTime >= endTime)
                throw new InvalidOperationException("Geçersiz randevu saati!");

        }
    }

}
