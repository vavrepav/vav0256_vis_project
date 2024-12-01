using AutoMapper;
using MailManagement_vav0256.Entities;
using MailManagement_vav0256.DTOs.EmailNotification;

namespace MailManagement_vav0256.Profiles
{
    public class EmailNotificationProfile : Profile
    {
        public EmailNotificationProfile()
        {
            CreateMap<EmailNotification, EmailNotificationReadDto>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));
            CreateMap<EmailNotificationCreateDto, EmailNotification>();
            CreateMap<EmailNotificationUpdateDto, EmailNotification>();
        }
    }
}