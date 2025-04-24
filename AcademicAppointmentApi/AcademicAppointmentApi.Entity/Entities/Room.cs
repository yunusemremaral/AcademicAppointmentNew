using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.EntityLayer.Entities
{
    public class Room
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<AppUser> AssignedInstructors { get; set; }
    }
}
