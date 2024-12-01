using MailManagement_vav0256.Services.Interfaces;
using MailManagement_vav0256.Repositories.Interfaces;
using MailManagement_vav0256.DTOs.Mail;
using MailManagement_vav0256.Entities;

namespace MailManagement_vav0256.Services
{
    public class MailService : IMailService
    {
        private readonly IMailRepository _mailRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailNotificationRepository _emailNotificationRepository;

        public MailService(IMailRepository mailRepository, IUserRepository userRepository, IEmailNotificationRepository emailNotificationRepository)
        {
            _mailRepository = mailRepository;
            _userRepository = userRepository;
            _emailNotificationRepository = emailNotificationRepository;
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
        
        public IEnumerable<MailReadDto> GetMailsByRecipientId(Guid recipientId)
        {
            var mails = _mailRepository.GetByRecipientId(recipientId);
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

        public MailReadDto CreateMail(MailCreateDto mailDto, Guid receptionistId)
        {
            var mail = new Mail
            {
                Id = Guid.NewGuid(),
                MailType = mailDto.MailType,
                Description = mailDto.Description,
                RecipientId = mailDto.RecipientId,
                SenderId = mailDto.SenderId,
                Status = "Received",
                ReceivedDate = DateTime.UtcNow,
                ReceptionistId = receptionistId
            };

            _mailRepository.Create(mail);
            
            var notification = new EmailNotification
            {
                Id = Guid.NewGuid(),
                UserId = mail.RecipientId,
                MailId = mail.Id,
                SentDate = DateTime.UtcNow,
                NotificationType = "Mail Received"
            };
            _emailNotificationRepository.Create(notification);

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
        
        public bool ClaimMail(MailClaimDto claimDto, Guid mailId)
        {
            var mail = _mailRepository.GetById(mailId);
            if (mail == null)
                return false;

            var user = _userRepository.GetByEmail(claimDto.EmployeeEmail);
            if (user == null)
                return false;

            mail.Status = "Claimed";
            mail.ClaimedDate = DateTime.UtcNow;

            _mailRepository.Update(mail);

            // Create EmailNotification
            var notification = new EmailNotification
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                MailId = mail.Id,
                SentDate = DateTime.UtcNow,
                NotificationType = "Mail Claimed"
            };
            _emailNotificationRepository.Create(notification);

            return true;
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