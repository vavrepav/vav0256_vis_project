using MailManagement_vav0256.DTOs.EmailNotification;

namespace MailManagement_vav0256.Services.Interfaces
{
    public interface IEmailNotificationService
    {
        IEnumerable<EmailNotificationReadDto> GetAllNotifications();
        EmailNotificationReadDto GetNotificationById(Guid id);
        EmailNotificationReadDto CreateNotification(EmailNotificationCreateDto notificationDto);
        bool UpdateNotification(Guid id, EmailNotificationUpdateDto notificationDto);
        bool DeleteNotification(Guid id);
    }
}