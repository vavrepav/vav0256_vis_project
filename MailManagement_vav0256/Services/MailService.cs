using AutoMapper;
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
        private readonly IMapper _mapper;
        
        public MailService(IMailRepository mailRepository, IUserRepository userRepository, IEmailNotificationRepository emailNotificationRepository, IMapper mapper)
        {
            _mailRepository = mailRepository;
            _userRepository = userRepository;
            _emailNotificationRepository = emailNotificationRepository;
            _mapper = mapper;
        }

        public IEnumerable<MailReadDto> GetAllMails()
        {
            var mails = _mailRepository.GetAll();
            return _mapper.Map<IEnumerable<MailReadDto>>(mails);
        }

        public IEnumerable<MailReadDto> GetMailsByRecipientId(Guid recipientId)
        {
            var mails = _mailRepository.GetByRecipientId(recipientId);
            return _mapper.Map<IEnumerable<MailReadDto>>(mails);
        }

        public MailReadDto GetMailById(Guid id)
        {
            var mail = _mailRepository.GetById(id);
            return _mapper.Map<MailReadDto>(mail);
        }

        public MailReadDto CreateMail(MailCreateDto mailDto, Guid receptionistId)
        {
            var mail = _mapper.Map<Mail>(mailDto);
            mail.Id = Guid.NewGuid();
            mail.Status = "Received";
            mail.ReceivedDate = DateTime.UtcNow;
            mail.ReceptionistId = receptionistId;

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

            return _mapper.Map<MailReadDto>(mail);
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

            _mapper.Map(mailDto, mail);

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