using AcademicAppointmentApi.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Abstract
{
    public interface INotificationService : IGenericService<Notification>
    {
        Task<List<Notification>> GetByUserIdAsync(string userId);
        Task<List<Notification>> GetUnreadByUserIdAsync(string userId);
    }
}
