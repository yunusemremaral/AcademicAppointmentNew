using AcademicAppointmentApi.EntityLayer.Entities;

namespace AcademicAppointmentApi.Presentation.Dtos
{
    public class AppointmentCreateDto
    {
        public string AcademicUserId { get; set; }
        public string StudentUserId { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public AppointmentStatus Status { get; set; }

    }
}
