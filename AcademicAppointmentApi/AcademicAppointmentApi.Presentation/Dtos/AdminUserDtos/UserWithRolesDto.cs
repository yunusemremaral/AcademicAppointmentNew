﻿namespace AcademicAppointmentApi.Presentation.Dtos.AdminUserDtos
{
    public class UserWithRolesDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
