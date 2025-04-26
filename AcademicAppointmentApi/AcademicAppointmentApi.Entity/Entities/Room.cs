﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.EntityLayer.Entities
{
    public class Room   
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }


}
