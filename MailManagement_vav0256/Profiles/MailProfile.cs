using AutoMapper;
using MailManagement_vav0256.Entities;
using MailManagement_vav0256.DTOs.Mail;

namespace MailManagement_vav0256.Profiles
{
    public class MailProfile : Profile
    {
        public MailProfile()
        {
            CreateMap<Mail, MailReadDto>();
            CreateMap<MailCreateDto, Mail>();
            CreateMap<MailUpdateDto, Mail>();
            CreateMap<MailClaimDto, Mail>();
        }
    }
}