using AutoMapper;
using MailManagement_vav0256.Entities;
using MailManagement_vav0256.DTOs.User;

namespace MailManagement_vav0256.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserReadDto>();
            CreateMap<UserCreateDto, User>();
            CreateMap<UserUpdateDto, User>();
        }
    }
}