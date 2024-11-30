using MailManagement_vav0256.Services.Interfaces;
using MailManagement_vav0256.Repositories.Interfaces;
using MailManagement_vav0256.DTOs.Mail;
using MailManagement_vav0256.Entities;

namespace MailManagement_vav0256.Services
{
    public class MailService : IMailService
    {
        private readonly IMailRepository _mailRepository;

        public MailService(IMailRepository mailRepository)
        {
            _mailRepository = mailRepository;
        }

        public IEnumerable<MailReadDto> GetAllMails()
        {
            var mails = _mailRepository.GetAll();
            return mails.Select(mail => new MailReadDto
            {
                Id = mail.Id,
                MailType = mail.MailType,
                Description = mail.Description,
                RecipientId = mail.RecipientId,
                SenderId = mail.SenderId,
                Status = mail.Status,
                ReceivedDate = mail.ReceivedDate,
                ClaimedDate = mail.ClaimedDate
            });
        }

        public MailReadDto GetMailById(Guid id)
        {
            var mail = _mailRepository.GetById(id);
            if (mail == null)
                return null;

            return new MailReadDto
            {
                Id = mail.Id,
                MailType = mail.MailType,
                Description = mail.Description,
                RecipientId = mail.RecipientId,
                SenderId = mail.SenderId,
                Status = mail.Status,
                ReceivedDate = mail.ReceivedDate,
                ClaimedDate = mail.ClaimedDate
            };
        }

        public MailReadDto CreateMail(MailCreateDto mailDto)
        {
            var mail = new Mail
            {
                Id = Guid.NewGuid(),
                MailType = mailDto.MailType,
                Description = mailDto.Description,
                RecipientId = mailDto.RecipientId,
                SenderId = mailDto.SenderId,
                Status = mailDto.Status,
                ReceivedDate = mailDto.ReceivedDate
            };

            var createdMail = _mailRepository.Create(mail);

            return new MailReadDto
            {
                Id = createdMail.Id,
                MailType = createdMail.MailType,
                Description = createdMail.Description,
                RecipientId = createdMail.RecipientId,
                SenderId = createdMail.SenderId,
                Status = createdMail.Status,
                ReceivedDate = createdMail.ReceivedDate,
                ClaimedDate = createdMail.ClaimedDate
            };
        }

        public bool UpdateMail(Guid id, MailUpdateDto mailDto)
        {
            var mail = _mailRepository.GetById(id);
            if (mail == null)
                return false;

            mail.MailType = mailDto.MailType;
            mail.Description = mailDto.Description;
            mail.RecipientId = mailDto.RecipientId;
            mail.SenderId = mailDto.SenderId;
            mail.Status = mailDto.Status;
            mail.ClaimedDate = mailDto.ClaimedDate;

            _mailRepository.Update(mail);

            return true;
        }

        public bool DeleteMail(Guid id)
        {
            var mail = _mailRepository.GetById(id);
            if (mail == null)
                return false;

            _mailRepository.Delete(id);

            return true;
        }
    }
}