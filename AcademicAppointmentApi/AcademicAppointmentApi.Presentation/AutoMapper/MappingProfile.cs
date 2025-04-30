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
            CreateMap<Department, DepartmentDto>().ReverseMap();
        }
    }
}
