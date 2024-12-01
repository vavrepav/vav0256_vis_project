using AutoMapper;
using MailManagement_vav0256.Services.Interfaces;
using MailManagement_vav0256.Repositories.Interfaces;
using MailManagement_vav0256.DTOs.Sender;
using MailManagement_vav0256.Entities;

namespace MailManagement_vav0256.Services
{
    public class SenderService : ISenderService
    {
        private readonly ISenderRepository _senderRepository;
        private readonly IMapper _mapper;

        public SenderService(ISenderRepository senderRepository, IMapper mapper)
        {
            _senderRepository = senderRepository;
            _mapper = mapper;
        }

        public IEnumerable<SenderReadDto> GetAllSenders()
        {
            var senders = _senderRepository.GetAll();
            return _mapper.Map<IEnumerable<SenderReadDto>>(senders);
        }

        public SenderReadDto GetSenderById(Guid id)
        {
            var sender = _senderRepository.GetById(id);
            return _mapper.Map<SenderReadDto>(sender);
        }

        public SenderReadDto CreateSender(SenderCreateDto senderDto)
        {
            var sender = _mapper.Map<Sender>(senderDto);
            sender.Id = Guid.NewGuid();

            _senderRepository.Create(sender);

            return _mapper.Map<SenderReadDto>(sender);
        }

        public bool UpdateSender(Guid id, SenderUpdateDto senderDto)
        {
            var sender = _senderRepository.GetById(id);
            if (sender == null)
                return false;

            _mapper.Map(senderDto, sender);

            _senderRepository.Update(sender);

            return true;
        }

        public bool DeleteSender(Guid id)
        {
            var sender = _senderRepository.GetById(id);
            if (sender == null)
                return false;

            _senderRepository.Delete(id);

            return true;
        }
    }
}