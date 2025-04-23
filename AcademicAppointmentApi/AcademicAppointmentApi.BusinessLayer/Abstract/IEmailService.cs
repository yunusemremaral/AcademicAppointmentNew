using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Abstract
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(string email, string confirmationLink);
        Task SendPasswordResetEmailAsync(string email, string resetLink);
    }
}
