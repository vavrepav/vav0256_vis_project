namespace MailManagement_vav0256.Entities
{
    public class Mail
    {
        public Guid Id { get; set; }
        public string MailType { get; set; }
        public string Description { get; set; }
        public Guid RecipientId { get; set; }
        public Guid SenderId { get; set; }
        public string Status { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime? ClaimedDate { get; set; }
        public Guid ReceptionistId { get; set; }

        public Sender Sender { get; set; }
        public User Recipient { get; set; }
        public User Receptionist { get; set; }
    }
}