using AcademicAppointmentApi.EntityLayer.Entities;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Abstract
{
    public interface ITokenService
    {
        Task<string> CreateAccessTokenAsync(AppUser user);
        string GenerateRefreshToken();
    }
}
