using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.EntityLayer.Entities
{
    public class School
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<Department> Departments { get; set; }
    }

}
