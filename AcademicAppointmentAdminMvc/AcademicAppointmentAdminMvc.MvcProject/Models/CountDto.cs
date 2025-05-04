using Newtonsoft.Json;

namespace AcademicAppointmentAdminMvc.MvcProject.Models
{
    public class CountDto
    {
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}
