using AcademicAppointmentApi.BusinessLayer.Abstract;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Concrete
{
    public class EmailService : IEmailService
    {
        private const string SenderEmail = "dolandiriciliklamucadeleet@gmail.com";
        private const string SenderPassword = "pddagdtfbczidkdq";
        private const string SmtpServer = "smtp.gmail.com";
        private const int SmtpPort = 587;

        public async Task SendConfirmationEmailAsync(string email, string confirmationLink)
        {
            string subject = "Email Doğrulama";
            string body = $"Lütfen e-posta adresinizi doğrulamak için aşağıdaki bağlantıya tıklayın:\n\n{confirmationLink}";
            await SendEmailAsync(email, subject, body);
        }

        public async Task SendPasswordResetEmailAsync(string email, string resetLink)
        {
            string subject = "Şifre Sıfırlama";
            string body = $"Şifrenizi sıfırlamak için aşağıdaki bağlantıya tıklayın:\n\n{resetLink}";
            await SendEmailAsync(email, subject, body);
        }

        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Academic Appointment", SenderEmail));
            emailMessage.To.Add(new MailboxAddress("", toEmail));
            emailMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder { TextBody = body };
            emailMessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(SmtpServer, SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(SenderEmail, SenderPassword);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
