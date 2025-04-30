using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentShare.Dtos.CourseDtos;
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
            CreateMap<Course, CourseDepartmentDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : string.Empty)) // Null kontrolü ekleyin
                .ReverseMap();

            CreateMap<Course, CourseDepartmentWithDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : string.Empty)) // Null kontrolü ekleyin
                .ReverseMap();

            CreateMap<Course, CourseListDto>()
    .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : string.Empty))
    .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor != null ? src.Instructor.UserName : string.Empty));

            CreateMap<Course, CourseWithDetailsDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : string.Empty)) // Null kontrolü ekleyin
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor != null ? src.Instructor.UserName : string.Empty)) // Null kontrolü ekleyin
                .ForMember(dest => dest.InstructorId, opt => opt.MapFrom(src => src.Instructor != null ? src.Instructor.Id.ToString() : string.Empty)) // Null kontrolü ekleyin
                .ReverseMap();

            CreateMap<Course, CourseWithDetailsWithDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : string.Empty)) // Null kontrolü ekleyin
                .ForMember(dest => dest.InstructorName, opt => opt.MapFrom(src => src.Instructor != null ? src.Instructor.UserName : string.Empty)) // Null kontrolü ekleyin
                .ForMember(dest => dest.InstructorId, opt => opt.MapFrom(src => src.Instructor != null ? src.Instructor.Id.ToString() : string.Empty)) // Null kontrolü ekleyin
                .ForMember(dest => dest.InstructorEmail, opt => opt.MapFrom(src => src.Instructor != null ? src.Instructor.Email : string.Empty)) // Null kontrolü ekleyin
                .ReverseMap();

        }
    }
}
