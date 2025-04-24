using AcademicAppointmentApi.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.DataAccessLayer.Abstract
{
    public interface INotificationRepository : IGenericRepository<Notification>
    {
        Task<List<Notification>> GetByUserIdAsync(string userId);
        Task<List<Notification>> GetUnreadByUserIdAsync(string userId);
    }
}
