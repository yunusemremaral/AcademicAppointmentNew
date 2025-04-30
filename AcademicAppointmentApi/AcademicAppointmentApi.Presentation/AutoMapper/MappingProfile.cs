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
            // SCHOOL MAPPINGS
            CreateMap<School, SchoolListDto>().ReverseMap();
            CreateMap<School, SchoolCreateDto>().ReverseMap();
            CreateMap<School, SchoolUpdateDto>().ReverseMap();
            CreateMap<School, SchoolDetailDto>()
                .ForMember(dest => dest.Departments, opt => opt.MapFrom(src => src.Departments))
                .ReverseMap();
            CreateMap<School, DepartmentSchoolDto>().ReverseMap();

            // DEPARTMENT MAPPINGS
            CreateMap<DepartmentCreateDto, Department>()
                .ForMember(dest => dest.School, opt => opt.Ignore()) // Eğer School'ı ayrıca eklemek gerekiyorsa
                .ForMember(dest => dest.Courses, opt => opt.Ignore()); // Eğer Courses ayrıca eklenmeli

            CreateMap<Department, DepartmentListDto>().ReverseMap();
            CreateMap<Department, DepartmentUpdateDto>().ReverseMap();
            CreateMap<Department, DepartmentDetailDto>()
                .ForMember(dest => dest.School, opt => opt.MapFrom(src => src.School))
                .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses))
                .ReverseMap();
            CreateMap<Department, DepartmentListWithCoursesDto>()
                .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses))
                .ReverseMap();

            // COURSE MAPPINGS
            CreateMap<Course, CourseDepartmentDto>().ReverseMap();

            // COURSE WITH DETAILS MAPPING
            CreateMap<Course, CourseWithDetailsDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name)) // DepartmentName için ilişkili isim
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor.UserName)) // InstructorName için ilişkili isim
                .ForMember(dest => dest.InstructorId, opt => opt.MapFrom(src => src.Instructor.Id.ToString())) // InstructorId
                .ReverseMap();
        }
    }
}
