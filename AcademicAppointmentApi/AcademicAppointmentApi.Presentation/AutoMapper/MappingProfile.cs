using AcademicAppointmentApi.EntityLayer.Entities;
using AcademicAppointmentShare.Dtos.AppointmentDtoS;
using AcademicAppointmentShare.Dtos.CourseDtos;
using AcademicAppointmentShare.Dtos.DepartmentDtos;
using AcademicAppointmentShare.Dtos.MessageDtos;
using AcademicAppointmentShare.Dtos.NotificationDtos;
using AcademicAppointmentShare.Dtos.RoleDtos;
using AcademicAppointmentShare.Dtos.RoomDtos;
using AcademicAppointmentShare.Dtos.SchoolDtos;
using AcademicAppointmentShare.Dtos.UserDtos;
using AutoMapper;

namespace AcademicAppointmentApi.Presentation.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region OKUL 
            // School <-> SchoolListDto
            CreateMap<School, SchoolListDto>().ReverseMap();

            // School <-> SchoolCreateDto (sadece tek yönlü map gerekli)
            CreateMap<SchoolCreateDto, School>();

            // School <-> SchoolUpdateDto (Update için iki yönlü map mantıklı)
            CreateMap<SchoolUpdateDto, School>().ReverseMap();

            // Department <-> DepartmentSchoolDto
            CreateMap<Department, SDepartmentSchoolDto>().ReverseMap();

            // School <-> SchoolDetailDto (nested mapping için önemli)
            CreateMap<School, SchoolDetailDto>()
                .ForMember(dest => dest.Departments, opt => opt.MapFrom(src => src.Departments));

            #endregion

            #region DEPARTMENT
            // Department Entity to Department DTO
            CreateMap<Department, DepartmentListDto>();
            CreateMap<Department, DepartmentDetailDto>()
                .ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.School.Name)); // Custom mapping for SchoolName
            CreateMap<Department, DepartmentListWithCoursesDto>()
                .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.Courses));

            // Department DTO to Department Entity
            CreateMap<DepartmentCreateDto, Department>().ReverseMap();
            CreateMap<DepartmentUpdateDto, Department>(); // This was missing, it should be added to resolve the error

            // DepartmentCourse DTO and Entity
            CreateMap<Course, DepartmentCourseDto>();

            // Map AppUser to DepartmentAppUserDto
            CreateMap<AppUser, DepartmentAppUserDto>();  // Map AppUser to DepartmentAppUserDto

            // Map Faculty Members (AppUser) to DepartmentAppUserDto
            CreateMap<Department, DepartmentDetailDto>()
                .ForMember(dest => dest.FacultyMembers, opt => opt.MapFrom(src => src.FacultyMembers));  // Map FacultyMembers to DepartmentAppUserDto

            CreateMap<Department, DepartmentWithSchoolDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.School.Name))
                .ForMember(dest => dest.SchoolId, opt => opt.MapFrom(src => src.School.Id));
            CreateMap<Course, DepartmentCourseWithInstructorDto>()
                .ForMember(dest => dest.InstructorUserName, opt => opt.MapFrom(src => src.Instructor.UserName))
                .ForMember(dest => dest.InstructorEmail, opt => opt.MapFrom(src => src.Instructor.Email))
                .ForMember(dest => dest.InstructorId, opt => opt.MapFrom(src => src.Instructor.Id)); // string'e uygun olacak şekilde


            #endregion

            #region COURSE

            CreateMap<Course, CourseListDto>().ReverseMap();
            CreateMap<Course, CourseCreateDto>().ReverseMap();
            CreateMap<Course, CourseUpdateDto>().ReverseMap();

            CreateMap<Course, CourseDetailDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.InstructorUserName, opt => opt.MapFrom(src => src.Instructor.UserName))
                .ForMember(dest => dest.InstructorEmail, opt => opt.MapFrom(src => src.Instructor.Email));

            CreateMap<Course, CourseWithInstructorAndDepartmentDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.Department.School.Name))
                .ForMember(dest => dest.InstructorUserName, opt => opt.MapFrom(src => src.Instructor.UserName))
                .ForMember(dest => dest.InstructorEmail, opt => opt.MapFrom(src => src.Instructor.Email));

            #endregion

            #region ROOM
            CreateMap<Room, RoomDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.AppUser.UserName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AppUser.Email));

            CreateMap<CreateRoomDto, Room>();
            CreateMap<UpdateRoomDto, Room>();

            #endregion

            #region NOTIFICATION
            CreateMap<Notification, NotificationDto>().ReverseMap();
            CreateMap<CreateNotificationDto, Notification>();
            CreateMap<UpdateNotificationDto, Notification>();

            #endregion

            #region MESSAGE
            CreateMap<CreateMessageDto, Message>();
            CreateMap<UpdateMessageDto, Message>();
            CreateMap<Message, ResultMessageDto>()
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Sender.UserName))
                .ForMember(dest => dest.ReceiverName, opt => opt.MapFrom(src => src.Receiver.UserName));

            #endregion

            #region APPOINTMENT
            CreateMap<AppointmentCreateDto, Appointment>();
            CreateMap<AppointmentUpdateDto, Appointment>();
            CreateMap<Appointment, AppointmentResultDto>()
                .ForMember(dest => dest.AcademicUserName, opt => opt.MapFrom(src => src.AcademicUser.UserName))
                .ForMember(dest => dest.StudentUserName, opt => opt.MapFrom(src => src.StudentUser.UserName));
            #endregion

            #region USER VE ROLE
            CreateMap<AppUser, UserDto>();
            CreateMap<CreateUserDto, AppUser>();
            CreateMap<UpdateUserDto, AppUser>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<AppRole, RoleDto>();
            CreateMap<AppUser, UserWithDetailsDto>()
                .ForMember(dest => dest.SchoolName, opt => opt.MapFrom(src => src.School != null ? src.School.Name : null))
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department != null ? src.Department.Name : null))
                .ForMember(dest => dest.RoomName, opt => opt.MapFrom(src => src.Room != null ? src.Room.Name : null));

            CreateMap<Course, UserCourseDto>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.InstructorFullName, opt => opt.MapFrom(src => src.Instructor.UserName ));

            #endregion
        }
    }
}
