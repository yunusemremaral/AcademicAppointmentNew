﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentShare.Dtos.UserDtos
{
    public class CreateUserDto
    {
        public string UserFullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? SchoolId { get; set; }
        public int? DepartmentId { get; set; }
    }

}
