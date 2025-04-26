using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Abstract
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? Email { get; }
        string? Role { get; }
        string? UserName { get; }
        string? SchoolId { get; }
        string? DepartmentId { get; }
    }

}
