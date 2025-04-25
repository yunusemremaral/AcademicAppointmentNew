namespace AcademicAppointmentApi.Presentation.Dtos.MessageDtos
{
    public class MessageDto
    {
        public string Id { get; set; }
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
    }
}
