namespace AcademicAppointmentApi.Presentation.Dtos.MessageDtos
{
    public class MessageCreateDto
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Content { get; set; }
    }
}
