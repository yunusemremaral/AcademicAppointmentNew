using AcademicAppointmentApi.BusinessLayer.Abstract;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AcademicAppointmentApi.BusinessLayer.Concrete
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
        public string? Role => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role);
        public string? UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue("username");
        public string? SchoolId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("schoolId");
        public string? DepartmentId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("departmentId");
    }
}
