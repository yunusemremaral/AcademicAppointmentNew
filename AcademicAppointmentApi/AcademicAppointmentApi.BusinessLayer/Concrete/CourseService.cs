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

        public async Task<IReadOnlyList<Course>> TGetByDepartmentIdAsync(int departmentId)
        {
            return await _courseRepository.GetByDepartmentIdAsync(departmentId);
        }

        // InstructorId'ye göre Course'ları al
        public async Task<IReadOnlyList<Course>> TGetByInstructorIdAsync(string instructorId)
        {
            return await _courseRepository.GetByInstructorIdAsync(instructorId);
        }

        // Course'u detaylarıyla birlikte al
        public async Task<Course> TGetCourseWithDetailsAsync(int courseId)
        {
            return await _courseRepository.GetCourseWithDetailsAsync(courseId);
        }

    }
}
