using AutoMapper;
using MailManagement_vav0256.Entities;
using MailManagement_vav0256.DTOs.Mail;

namespace MailManagement_vav0256.Profiles
{
    public class MailProfile : Profile
    {
        public MailProfile()
        {
            CreateMap<Mail, MailReadDto>()
                .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.Sender))
                .ForMember(dest => dest.Recipient, opt => opt.MapFrom(src => src.Recipient))
                .ForMember(dest => dest.Receptionist, opt => opt.MapFrom(src => src.Receptionist));
            CreateMap<MailCreateDto, Mail>();
            CreateMap<MailUpdateDto, Mail>();
        }
    }
}