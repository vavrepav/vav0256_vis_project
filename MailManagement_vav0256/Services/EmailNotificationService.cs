using MailManagement_vav0256.Services.Interfaces;
using MailManagement_vav0256.Repositories.Interfaces;
using MailManagement_vav0256.DTOs.EmailNotification;
using MailManagement_vav0256.Entities;
using AutoMapper;

namespace MailManagement_vav0256.Services
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly IEmailNotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public EmailNotificationService(IEmailNotificationRepository notificationRepository, IMapper mapper)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
        }

        public IEnumerable<EmailNotificationReadDto> GetAllNotifications()
        {
            var notifications = _notificationRepository.GetAll();
            return _mapper.Map<IEnumerable<EmailNotificationReadDto>>(notifications);
        }

        public EmailNotificationReadDto GetNotificationById(Guid id)
        {
            var notification = _notificationRepository.GetById(id);
            return _mapper.Map<EmailNotificationReadDto>(notification);
        }

        public EmailNotificationReadDto CreateNotification(EmailNotificationCreateDto notificationDto)
        {
            var notification = _mapper.Map<EmailNotification>(notificationDto);
            notification.Id = Guid.NewGuid();
            notification.SentDate = DateTime.UtcNow;

            _notificationRepository.Create(notification);

            return _mapper.Map<EmailNotificationReadDto>(notification);
        }

        public bool UpdateNotification(Guid id, EmailNotificationUpdateDto notificationDto)
        {
            var notification = _notificationRepository.GetById(id);
            if (notification == null)
                return false;

            _mapper.Map(notificationDto, notification);

            _notificationRepository.Update(notification);

            return true;
        }

        public bool DeleteNotification(Guid id)
        {
            var notification = _notificationRepository.GetById(id);
            if (notification == null)
                return false;

            _notificationRepository.Delete(id);

            return true;
        }
    }
}