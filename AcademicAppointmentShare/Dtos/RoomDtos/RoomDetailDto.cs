using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentShare.Dtos.RoomDtos
{
    public class RoomDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? AppUserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
}
