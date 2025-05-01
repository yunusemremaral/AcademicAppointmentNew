using AcademicAppointmentApi.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.DataAccessLayer.Abstract
{
    public interface IRoomRepository : IGenericRepository<Room>
    {
        Task<List<Room>> GetAllWithUsersAsync();
    Task<Room?> GetByIdWithUserAsync(int id);
    Task<Room?> GetByUserIdAsync(string userId);
}

}
