using MailManagement_vav0256.DTOs.User;

namespace MailManagement_vav0256.DTOs.EmailNotification
{
    public class EmailNotificationReadDto
    {
        public Guid Id { get; set; }
        public UserReadDto User { get; set; }
        public Guid MailId { get; set; }
        public DateTime SentDate { get; set; }
        public string NotificationType { get; set; }
    }
}