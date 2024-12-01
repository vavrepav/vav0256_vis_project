using AutoMapper;
using MailManagement_vav0256.Entities;
using MailManagement_vav0256.DTOs.EmailNotification;

namespace MailManagement_vav0256.Profiles
{
    public class EmailNotificationProfile : Profile
    {
        public EmailNotificationProfile()
        {
            CreateMap<EmailNotification, EmailNotificationReadDto>();
            CreateMap<EmailNotificationCreateDto, EmailNotification>();
            CreateMap<EmailNotificationUpdateDto, EmailNotification>();
        }
    }
}