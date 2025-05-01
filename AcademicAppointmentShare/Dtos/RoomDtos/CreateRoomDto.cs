using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentShare.Dtos.RoomDtos
{
    public class CreateRoomDto
    {
        public string Name { get; set; }
        public string? AppUserId { get; set; }
    }
}
