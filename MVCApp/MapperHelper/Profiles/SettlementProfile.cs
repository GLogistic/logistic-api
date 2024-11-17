using AutoMapper;
using Entities;
using Entities.Models.DTOs;

namespace MapperHelper.Profiles
{
    public class SettlementProfile : Profile
    {
        public SettlementProfile()
        {
            CreateMap<Settlement, SettlementDto>();
            CreateMap<SettlementCreateDto, Settlement>().ReverseMap();
            CreateMap<SettlementUpdateDto, Settlement>().ReverseMap();
        }
    }
}
