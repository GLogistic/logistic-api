using AutoMapper;
using Entities;
using Entities.Models.DTOs;

namespace MapperHelper.Profiles
{
    public class CargoProfile : Profile
    {
        public CargoProfile()
        {
            CreateMap<Cargo, CargoDto>();
            CreateMap<CargoCreateDto, Cargo>().ReverseMap();
            CreateMap<CargoUpdateDto, Cargo>().ReverseMap();
        }
    }
}
