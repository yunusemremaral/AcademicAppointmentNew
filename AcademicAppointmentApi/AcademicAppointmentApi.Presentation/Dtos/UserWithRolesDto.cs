﻿namespace AcademicAppointmentApi.Presentation.Dtos
{
    public class UserWithRolesDto
    {
        public string Id { get; set; }
        public string UserFullName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; } = new();
    }
}
