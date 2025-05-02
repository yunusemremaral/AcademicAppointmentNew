using AcademicAppointmentApi.BusinessLayer.Abstract;
using AcademicAppointmentApi.DataAccessLayer.Abstract;
using AcademicAppointmentApi.EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcademicAppointmentApi.BusinessLayer.Concrete
{
    public class DepartmentService : TGenericService<Department>, IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository) : base(departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<IReadOnlyList<Department>> TGetAllWithCoursesAsync()
        {
            return await _departmentRepository.GetAllWithCoursesAsync();
        }
        public async Task<IReadOnlyList<Department>> TGetDepartmentsWithSchoolAsync()
        {
            return await _departmentRepository.GetDepartmentsWithSchoolAsync();
        }


        public async Task<IReadOnlyList<Course>> TGetCoursesByDepartmentIdAsync(int departmentId)
        {
            return await _departmentRepository.GetCoursesByDepartmentIdAsync(departmentId);
        }

        public async Task<Department?> TGetByIdWithDetailsAsync(int id)
        {
            return await _departmentRepository.GetByIdWithDetailsAsync(id);
        }

        public async Task<int> TGetCourseCountAsync(int departmentId)
        {
            return await _departmentRepository.GetCourseCountAsync(departmentId);
        }

        public async Task<IReadOnlyList<Course>> TGetCoursesWithInstructorByDepartmentIdAsync(int departmentId)
        {
            return await _departmentRepository.GetCoursesWithInstructorByDepartmentIdAsync(departmentId);
        }
    }
}
