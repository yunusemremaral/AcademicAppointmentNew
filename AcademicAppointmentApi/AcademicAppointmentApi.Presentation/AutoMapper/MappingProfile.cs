using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentShare.Dtos.DepartmentDtos;
using AcademicAppointmentShare.Dtos.SchoolDtos;
using AutoMapper;

namespace AcademicAppointmentApi.Presentation.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // School <-> Dto eşleşmeleri
            CreateMap<School, SchoolListDto>().ReverseMap();
            CreateMap<School, SchoolCreateDto>().ReverseMap();
            CreateMap<School, SchoolUpdateDto>().ReverseMap();
            CreateMap<School, SchoolDetailDto>().ReverseMap();

            // Department
            CreateMap<Department, DepartmentSchoolDto>().ReverseMap();
            // Department Entity to DTO
            CreateMap<Department, DepartmentDto>()
                .ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.School.Name));

            CreateMap<Department, DepartmentWithCoursesDto>()
                .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses));

            CreateMap<Department, DepartmentWithFacultyMembersDto>()
                .ForMember(dest => dest.FacultyMembers, opt => opt.MapFrom(src => src.FacultyMembers));

            CreateMap<Department, DepartmentWithStudentsDto>()
                .ForMember(dest => dest.Students, opt => opt.MapFrom(src => src.StudentMembers));

            // DTO to Entity
            CreateMap<DepartmentCreateDto, Department>();
            CreateMap<DepartmentUpdateDto, Department>();

        }
    }
}
