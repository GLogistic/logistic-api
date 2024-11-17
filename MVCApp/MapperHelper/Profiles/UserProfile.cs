using AutoMapper;
using Entities;
using Entities.Models.DTOs.User;

namespace MapperHelper.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRegistrationDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<User, UserUpdateDto>().ReverseMap();
        }
    }
}
