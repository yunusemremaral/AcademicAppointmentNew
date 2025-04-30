using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.DataAccessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Concrete
{
    public class CourseService : TGenericService<Course>, ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository) : base(courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<Course?> GetByIdWithDetailsAsync(int id)
        {
            return await _courseRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task<List<Course>> GetAllWithDetailsAsync()
        {
            return await _courseRepository.GetAllWithDetailsAsync();
        }

        public async Task<List<Course>> GetAllByInstructorIdWithDetailsAsync(string instructorId)
        {
            return await _courseRepository.GetAllByInstructorIdWithDetailsAsync(instructorId);
        }

    }
}
