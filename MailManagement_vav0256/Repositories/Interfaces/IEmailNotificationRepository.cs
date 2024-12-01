using MailManagement_vav0256.Entities;
using Microsoft.Data.SqlClient;

namespace MailManagement_vav0256.Repositories.Interfaces
{
    public interface IEmailNotificationRepository
    {
        IEnumerable<EmailNotification> GetAll();
        EmailNotification GetById(Guid id);
        EmailNotification Create(EmailNotification notification);

        public EmailNotification Create(EmailNotification notification, SqlConnection connection = null,
            SqlTransaction transaction = null);
        void Update(EmailNotification notification);
        void Delete(Guid id);
    }
}