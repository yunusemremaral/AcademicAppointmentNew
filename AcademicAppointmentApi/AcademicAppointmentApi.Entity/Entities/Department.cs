using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.EntityLayer.Entities
{
    public class Department
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string SchoolId { get; set; }
        public School School { get; set; }

        // Oda ilişkisi
        public ICollection<Room> Rooms { get; set; }

        // Hocalar
        public ICollection<AppUser> FacultyMembers { get; set; }

        // Dersler
        public ICollection<Course> Courses { get; set; }
    }


}