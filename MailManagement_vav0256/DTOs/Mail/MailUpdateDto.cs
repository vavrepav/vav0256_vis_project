namespace MailManagement_vav0256.DTOs.Mail
{
    public class MailUpdateDto
    {
        public string MailType { get; set; }
        public string Description { get; set; }
        public Guid RecipientId { get; set; }
        public Guid SenderId { get; set; }
    }
}