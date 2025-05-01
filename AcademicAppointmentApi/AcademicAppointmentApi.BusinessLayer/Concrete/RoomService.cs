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
    public class RoomService : TGenericService<Room>, IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(IRoomRepository roomRepository) : base(roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<List<Room>> TGetAllWithUsersAsync()
        {
            return await _roomRepository.GetAllWithUsersAsync();
        }

        public async Task<Room?> TGetByIdWithUserAsync(int id)
        {
            return await _roomRepository.GetByIdWithUserAsync(id);
        }

        public async Task<Room?> TGetByUserIdAsync(string userId)
        {
            return await _roomRepository.GetByUserIdAsync(userId);
        }
    }
}
