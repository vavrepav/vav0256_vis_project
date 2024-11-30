using MailManagement_vav0256.Services.Interfaces;
using MailManagement_vav0256.Repositories.Interfaces;
using MailManagement_vav0256.DTOs.Sender;
using MailManagement_vav0256.Entities;

namespace MailManagement_vav0256.Services
{
    public class SenderService : ISenderService
    {
        private readonly ISenderRepository _senderRepository;

        public SenderService(ISenderRepository senderRepository)
        {
            _senderRepository = senderRepository;
        }

        public IEnumerable<SenderReadDto> GetAllSenders()
        {
            var senders = _senderRepository.GetAll();
            return senders.Select(sender => new SenderReadDto
            {
                Id = sender.Id,
                Name = sender.Name,
                ContactInfo = sender.ContactInfo
            });
        }

        public SenderReadDto GetSenderById(Guid id)
        {
            var sender = _senderRepository.GetById(id);
            if (sender == null)
                return null;

            return new SenderReadDto
            {
                Id = sender.Id,
                Name = sender.Name,
                ContactInfo = sender.ContactInfo
            };
        }

        public SenderReadDto CreateSender(SenderCreateDto senderDto)
        {
            var sender = new Sender
            {
                Id = Guid.NewGuid(),
                Name = senderDto.Name,
                ContactInfo = senderDto.ContactInfo
            };

            var createdSender = _senderRepository.Create(sender);

            return new SenderReadDto
            {
                Id = createdSender.Id,
                Name = createdSender.Name,
                ContactInfo = createdSender.ContactInfo
            };
        }

        public bool UpdateSender(Guid id, SenderUpdateDto senderDto)
        {
            var sender = _senderRepository.GetById(id);
            if (sender == null)
                return false;

            sender.Name = senderDto.Name;
            sender.ContactInfo = senderDto.ContactInfo;

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