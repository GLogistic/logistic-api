using AutoMapper;
using Entities;
using Entities.Models.DTOs;

namespace MapperHelper.Profiles
{
    public class RouteProfile : Profile
    {
        public RouteProfile()
        {
            CreateMap<Route, RouteDto>();
            CreateMap<RouteCreateDto, Route>().ReverseMap();
            CreateMap<RouteUpdateDto, Route>().ReverseMap();
        }
    }
}
