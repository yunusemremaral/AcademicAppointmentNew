﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentShare.Dtos.SchoolDtos
{
    public class SchoolDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<SDepartmentSchoolDto> Departments { get; set; }
    }

}

