namespace AcademicAppointmentAdminMvc.MvcProject.Dtos.UserMvcDtos
{
    public class UserDtoSchoolDepartment
    {
        public string Id { get; set; }       // Kullanıcı ID'si
        public string UserName { get; set; } // Kullanıcı adı
        public string Email { get; set; }    // E-posta adresi
        public string School { get; set; }   // Okul adı
        public string Department { get; set; } // Bölüm adı
    }
}
