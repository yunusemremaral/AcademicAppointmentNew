namespace AcademicAppointmentAdminMvc.MvcProject.Dtos.UserMvcDtos
{
    public class UserCreateDto
    {
        public string UserName { get; set; } // Kullanıcı adı
        public string Email { get; set; }    // E-posta adresi
        public string Password { get; set; } // Kullanıcı şifresi
    }
}
