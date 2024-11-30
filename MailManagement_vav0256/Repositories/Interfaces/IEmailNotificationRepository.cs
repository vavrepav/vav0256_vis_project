using MailManagement_vav0256.Entities;

namespace MailManagement_vav0256.Repositories.Interfaces
{
    public interface IEmailNotificationRepository
    {
        IEnumerable<EmailNotification> GetAll();
        EmailNotification GetById(Guid id);
        EmailNotification Create(EmailNotification notification);
        void Update(EmailNotification notification);
        void Delete(Guid id);
    }
}