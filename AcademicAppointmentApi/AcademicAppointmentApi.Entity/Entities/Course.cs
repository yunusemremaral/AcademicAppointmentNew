using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.EntityLayer.Entities
{
    public class Course
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string DepartmentId { get; set; }
        public Department Department { get; set; }
        public string InstructorId { get; set; } // Öğretim üyesi
        public AppUser Instructor { get; set; }
    }
}
