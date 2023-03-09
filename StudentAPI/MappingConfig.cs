using AutoMapper;
using StudentAPI.Model;

namespace StudentAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig() {
            CreateMap<Student, StudentDTO>().ReverseMap();
            CreateMap<Student, StudentCreateDTO>().ReverseMap();
            CreateMap<Student, StudentUpdateDTO>().ReverseMap();

        }

    }
}
