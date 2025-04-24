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
    public class NotificationService : GenericService<Notification>, INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository) : base(notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<List<Notification>> GetByUserIdAsync(string userId)
        {
            return await _notificationRepository.GetByUserIdAsync(userId);
        }

        public async Task<List<Notification>> GetUnreadByUserIdAsync(string userId)
        {
            return await _notificationRepository.GetUnreadByUserIdAsync(userId);
        }
    }

}
