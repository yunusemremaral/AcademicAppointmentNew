using AcademicAppointmentApi.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.DataAccessLayer.Abstract
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<Course?> GetByIdWithDetailsAsync(int id);
        Task<List<Course>> GetAllWithDetailsAsync();
        Task<List<Course>> GetAllByInstructorIdWithDetailsAsync(string instructorId);


    }
}
