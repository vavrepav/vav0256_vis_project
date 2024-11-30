using MailManagement_vav0256.Services.Interfaces;
using MailManagement_vav0256.Repositories.Interfaces;
using MailManagement_vav0256.DTOs.EmailNotification;
using MailManagement_vav0256.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MailManagement_vav0256.Services
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly IEmailNotificationRepository _notificationRepository;

        public EmailNotificationService(IEmailNotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public IEnumerable<EmailNotificationReadDto> GetAllNotifications()
        {
            var notifications = _notificationRepository.GetAll();
            return notifications.Select(notification => new EmailNotificationReadDto
            {
                Id = notification.Id,
                UserId = notification.UserId,
                MailId = notification.MailId,
                SentDate = notification.SentDate,
                NotificationType = notification.NotificationType
            });
        }

        public EmailNotificationReadDto GetNotificationById(Guid id)
        {
            var notification = _notificationRepository.GetById(id);
            if (notification == null)
                return null;

            return new EmailNotificationReadDto
            {
                Id = notification.Id,
                UserId = notification.UserId,
                MailId = notification.MailId,
                SentDate = notification.SentDate,
                NotificationType = notification.NotificationType
            };
        }

        public EmailNotificationReadDto CreateNotification(EmailNotificationCreateDto notificationDto)
        {
            var notification = new EmailNotification
            {
                Id = Guid.NewGuid(),
                UserId = notificationDto.UserId,
                MailId = notificationDto.MailId,
                SentDate = DateTime.UtcNow,
                NotificationType = notificationDto.NotificationType
            };

            var createdNotification = _notificationRepository.Create(notification);

            return new EmailNotificationReadDto
            {
                Id = createdNotification.Id,
                UserId = createdNotification.UserId,
                MailId = createdNotification.MailId,
                SentDate = createdNotification.SentDate,
                NotificationType = createdNotification.NotificationType
            };
        }

        public bool UpdateNotification(Guid id, EmailNotificationUpdateDto notificationDto)
        {
            var notification = _notificationRepository.GetById(id);
            if (notification == null)
                return false;

            notification.NotificationType = notificationDto.NotificationType;

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