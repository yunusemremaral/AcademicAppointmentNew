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

        public async Task<List<Course>> TGetCoursesByDepartmentIdAsync(int departmentId)
        {
            return await _courseRepository.GetCoursesByDepartmentIdAsync(departmentId);
        }

        public async Task<List<Course>> TGetCoursesByInstructorIdAsync(string instructorId)
        {
            return await _courseRepository.GetCoursesByInstructorIdAsync(instructorId);
        }

        public async Task<List<Course>> TGetCoursesWithDepartmentAndInstructorAsync()
        {
            return await _courseRepository.GetCoursesWithDepartmentAndInstructorAsync();

        }

        public async Task<Course> TGetCourseWithDetailsByIdAsync(int id)
        {
            return await _courseRepository.GetCourseWithDetailsByIdAsync(id);
        }
    }
}
