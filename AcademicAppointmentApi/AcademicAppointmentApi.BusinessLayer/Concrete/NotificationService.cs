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
    public class NotificationService : TGenericService<Notification>, INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository) : base(notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<List<Notification>> TGetNotificationsByUserIdAsync(string userId)
        {
            return await _notificationRepository.GetNotificationsByUserIdAsync(userId);
        }
    }

}
