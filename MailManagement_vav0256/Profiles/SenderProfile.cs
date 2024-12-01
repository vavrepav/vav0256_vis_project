using AutoMapper;
using MailManagement_vav0256.Entities;
using MailManagement_vav0256.DTOs.Sender;

namespace MailManagement_vav0256.Profiles
{
    public class SenderProfile : Profile
    {
        public SenderProfile()
        {
            CreateMap<Sender, SenderReadDto>();
            CreateMap<SenderCreateDto, Sender>();
            CreateMap<SenderUpdateDto, Sender>();
        }
    }
}