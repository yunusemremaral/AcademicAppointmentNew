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
    public class AppointmentService : TGenericService<Appointment>, IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository) : base(appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }

        public async Task<List<Appointment>> TGetAppointmentsByStudentIdAsync(string studentId)
        {
            return await _appointmentRepository.GetAppointmentsByStudentIdAsync(studentId);
        }

        public async Task<List<Appointment>> TGetAppointmentsByAcademicIdAsync(string academicId)
        {
            return await _appointmentRepository.GetAppointmentsByAcademicIdAsync(academicId);
        }
    }
}
