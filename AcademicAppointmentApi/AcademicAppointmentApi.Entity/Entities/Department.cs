using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.EntityLayer.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int SchoolId { get; set; }
        public School School { get; set; }

        public ICollection<AppUser> FacultyMembers { get; set; }
        public ICollection<Course> Courses { get; set; }
    }


}