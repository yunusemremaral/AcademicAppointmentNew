﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentShare.Dtos.MessageDtos
{
    public class CreateMessageDto
    {
        public string Content { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
    }
}
