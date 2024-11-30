using MailManagement_vav0256.Repositories.Interfaces;
using MailManagement_vav0256.Entities;
using Microsoft.Data.SqlClient;

namespace MailManagement_vav0256.Repositories
{
    public class EmailNotificationRepository(string connectionString) : IEmailNotificationRepository
    {
        public IEnumerable<EmailNotification> GetAll()
        {
            var notifications = new List<EmailNotification>();
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand("SELECT * FROM EmailNotifications", connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                notifications.Add(MapReaderToNotification(reader));
            }
            return notifications;
        }

        public EmailNotification GetById(Guid id)
        {
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand("SELECT * FROM EmailNotifications WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapReaderToNotification(reader);
            }
            return null;
        }

        public EmailNotification Create(EmailNotification notification)
        {
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand("INSERT INTO EmailNotifications (Id, UserId, MailId, SentDate, NotificationType) VALUES (@Id, @UserId, @MailId, @SentDate, @NotificationType)", connection);
            command.Parameters.AddWithValue("@Id", notification.Id);
            command.Parameters.AddWithValue("@UserId", notification.UserId);
            command.Parameters.AddWithValue("@MailId", notification.MailId);
            command.Parameters.AddWithValue("@SentDate", notification.SentDate);
            command.Parameters.AddWithValue("@NotificationType", notification.NotificationType);

            connection.Open();
            command.ExecuteNonQuery();

            return notification;
        }

        public void Update(EmailNotification notification)
        {
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand("UPDATE EmailNotifications SET NotificationType = @NotificationType WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@NotificationType", notification.NotificationType);
            command.Parameters.AddWithValue("@Id", notification.Id);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void Delete(Guid id)
        {
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand("DELETE FROM EmailNotifications WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            command.ExecuteNonQuery();
        }

        private EmailNotification MapReaderToNotification(SqlDataReader reader)
        {
            return new EmailNotification
            {
                Id = Guid.Parse(reader["Id"].ToString()),
                UserId = Guid.Parse(reader["UserId"].ToString()),
                MailId = Guid.Parse(reader["MailId"].ToString()),
                SentDate = DateTime.Parse(reader["SentDate"].ToString()),
                NotificationType = reader["NotificationType"].ToString()
            };
        }
    }
}