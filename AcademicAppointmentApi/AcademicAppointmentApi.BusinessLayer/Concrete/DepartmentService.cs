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

        public async Task<IReadOnlyList<Department>> TGetAllWithFacultyMembersAsync()
        {
            return await _departmentRepository.GetAllWithFacultyMembersAsync();
        }

        public async Task<IReadOnlyList<Department>> TGetAllWithStudentsAsync()
        {
            return await _departmentRepository.GetAllWithStudentsAsync();
        }

        public async Task<IReadOnlyList<Department>> TGetDepartmentsBySchoolIdAsync(int schoolId)
        {
            return await _departmentRepository.GetDepartmentsBySchoolIdAsync(schoolId);
        }

        public async Task<IReadOnlyList<Department>> TSearchDepartmentsByNameAsync(string name)
        {
            return await _departmentRepository.SearchDepartmentsByNameAsync(name);
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

        public async Task<int> TGetFacultyMemberCountAsync(int departmentId)
        {
            return await _departmentRepository.GetFacultyMemberCountAsync(departmentId);
        }
    }
}
