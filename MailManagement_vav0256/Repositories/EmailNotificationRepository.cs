using MailManagement_vav0256.Repositories.Interfaces;
using MailManagement_vav0256.Entities;
using Microsoft.Data.SqlClient;

namespace MailManagement_vav0256.Repositories
{
    public class EmailNotificationRepository(string connectionString) : IEmailNotificationRepository
    {
        private readonly string logFilePath = "EmailNotificationRepository.txt";
        public IEnumerable<EmailNotification> GetAll()
        {
            var notifications = new List<EmailNotification>();
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand(@"SELECT en.*, u.Id AS UserId, u.Email AS UserEmail, u.Role AS UserRole
                FROM EmailNotifications en
                LEFT JOIN Users u ON en.UserId = u.Id", connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                notifications.Add(MapReaderToNotificationWithDetails(reader));
            }
            return notifications;
        }

        public EmailNotification GetById(Guid id)
        {
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand(@"SELECT en.*, u.Id AS UserId, u.Email AS UserEmail, u.Role AS UserRole
                FROM EmailNotifications en
                LEFT JOIN Users u ON en.UserId = u.Id
                WHERE en.Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapReaderToNotificationWithDetails(reader);
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
            
            LogChange("Create", notification);

            return notification;
        }
        
        public EmailNotification Create(EmailNotification notification, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var closeConnection = false;
            if (connection == null)
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                transaction = connection.BeginTransaction();
                closeConnection = true;
            }

            var command = new SqlCommand("INSERT INTO EmailNotifications (Id, UserId, MailId, SentDate, NotificationType) VALUES (@Id, @UserId, @MailId, @SentDate, @NotificationType)", connection, transaction);
            command.Parameters.AddWithValue("@Id", notification.Id);
            command.Parameters.AddWithValue("@UserId", notification.UserId);
            command.Parameters.AddWithValue("@MailId", notification.MailId);
            command.Parameters.AddWithValue("@SentDate", notification.SentDate);
            command.Parameters.AddWithValue("@NotificationType", notification.NotificationType);

            command.ExecuteNonQuery();

            LogChange("Create", notification);

            if (!closeConnection) return notification;
            transaction.Commit();
            connection.Close();

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
            
            LogChange("Update", notification);
        }

        public void Delete(Guid id)
        {
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand("DELETE FROM EmailNotifications WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            command.ExecuteNonQuery();
            
            LogChange("Delete", new EmailNotification { Id = id });
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
        
        private EmailNotification MapReaderToNotificationWithDetails(SqlDataReader reader)
        {
            var notification = new EmailNotification
            {
                Id = Guid.Parse(reader["Id"].ToString()),
                UserId = Guid.Parse(reader["UserId"].ToString()),
                MailId = Guid.Parse(reader["MailId"].ToString()),
                SentDate = DateTime.Parse(reader["SentDate"].ToString()),
                NotificationType = reader["NotificationType"].ToString()
            };

            notification.User = new User
            {
                Id = reader["UserId"] == DBNull.Value ? Guid.Empty : Guid.Parse(reader["UserId"].ToString()),
                Email = reader["UserEmail"] == DBNull.Value ? null : reader["UserEmail"].ToString(),
                Role = reader["UserRole"] == DBNull.Value ? null : reader["UserRole"].ToString()
            };

            return notification;
        }
        
        private void LogChange(string operation, EmailNotification notification)
        {
            string logMessage = $"{DateTime.Now}: {operation} - Notification ID: {notification.Id}, Type: {notification.NotificationType}";
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }
    }
}