using AcademicAppointmentApi.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Abstract
{
    public interface IRoomService : ITGenericService<Room>
    {
        Task<List<Room>> TGetAllWithUsersAsync();
        Task<Room?> TGetByIdWithUserAsync(int id);
        Task<Room?> TGetByUserIdAsync(string userId);
    }


}
