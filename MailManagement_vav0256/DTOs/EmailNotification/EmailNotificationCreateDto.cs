namespace MailManagement_vav0256.DTOs.EmailNotification
{
    public class EmailNotificationCreateDto
    {
        public Guid UserId { get; set; }
        public Guid MailId { get; set; }
        public string NotificationType { get; set; }
    }
}