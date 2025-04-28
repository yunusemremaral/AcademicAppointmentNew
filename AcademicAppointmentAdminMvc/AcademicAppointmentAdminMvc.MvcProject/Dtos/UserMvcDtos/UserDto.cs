using System.Text.Json.Serialization;

namespace AcademicAppointmentAdminMvc.MvcProject.Dtos.UserMvcDtos
{
    public class UserDto
    {
        public string Id { get; set; }       // Kullanıcı ID'si

        [JsonPropertyName("userName")]
        public string UserName { get; set; } // Kullanıcı adı

        [JsonPropertyName("email")]
        public string Email { get; set; }    // E-posta adresi

        [JsonPropertyName("schoolName")]
        public string School { get; set; }   // Okul adı

        [JsonPropertyName("departmentName")]
        public string Department { get; set; } // Bölüm adı

        public string Room { get; set; }     // Oda adı
    }





}
