using AcademicAppointmentApi.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Abstract
{
    public interface IRoomService : IGenericService<Room>
    {
        Task<List<Room>> GetRoomsByUserIdAsync(string userId);
    }
}
