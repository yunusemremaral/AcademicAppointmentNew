﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentShare.Dtos.DepartmentDtos
{
    public class DepartmentListWithCoursesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<DepartmentCourseDto> Courses { get; set; }


    }

}
