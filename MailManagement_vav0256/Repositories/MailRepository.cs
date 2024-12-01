using MailManagement_vav0256.Repositories.Interfaces;
using MailManagement_vav0256.Entities;
using Microsoft.Data.SqlClient;

namespace MailManagement_vav0256.Repositories
{
    public class MailRepository(string connectionString) : IMailRepository
    {
        private readonly string logFilePath = "MailRepository.txt";
        public IEnumerable<Mail> GetAll()
        {
            var mails = new List<Mail>();
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand("SELECT * FROM Mails", connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                mails.Add(MapReaderToMail(reader));
            }
            return mails;
        }

        public Mail GetById(Guid id)
        {
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand("SELECT * FROM Mails WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapReaderToMail(reader);
            }
            return null;
        }
        
        public IEnumerable<Mail> GetByRecipientId(Guid recipientId)
        {
            var mails = new List<Mail>();
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand("SELECT * FROM Mails WHERE RecipientId = @RecipientId", connection);
            command.Parameters.AddWithValue("@RecipientId", recipientId);

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                mails.Add(MapReaderToMail(reader));
            }
            return mails;
        }

        public Mail Create(Mail mail)
        {
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand(@"INSERT INTO Mails 
                (Id, MailType, Description, RecipientId, SenderId, Status, ReceivedDate, ClaimedDate, ReceptionistId) 
                VALUES (@Id, @MailType, @Description, @RecipientId, @SenderId, @Status, @ReceivedDate, @ClaimedDate, @ReceptionistId)", connection);
            command.Parameters.AddWithValue("@Id", mail.Id);
            command.Parameters.AddWithValue("@MailType", mail.MailType);
            command.Parameters.AddWithValue("@Description", mail.Description);
            command.Parameters.AddWithValue("@RecipientId", mail.RecipientId);
            command.Parameters.AddWithValue("@SenderId", mail.SenderId);
            command.Parameters.AddWithValue("@Status", mail.Status.ToString()); // Store enum as string
            command.Parameters.AddWithValue("@ReceivedDate", mail.ReceivedDate);
            command.Parameters.AddWithValue("@ClaimedDate", (object)mail.ClaimedDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@ReceptionistId", mail.ReceptionistId);

            connection.Open();
            command.ExecuteNonQuery();
            
            LogChange("Create", mail);

            return mail;
        }

        public void Update(Mail mail)
        {
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand("UPDATE Mails SET MailType = @MailType, Description = @Description, RecipientId = @RecipientId, SenderId = @SenderId, Status = @Status, ClaimedDate = @ClaimedDate WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@MailType", mail.MailType);
            command.Parameters.AddWithValue("@Description", mail.Description);
            command.Parameters.AddWithValue("@RecipientId", mail.RecipientId);
            command.Parameters.AddWithValue("@SenderId", mail.SenderId);
            command.Parameters.AddWithValue("@Status", mail.Status);
            command.Parameters.AddWithValue("@ClaimedDate", (object)mail.ClaimedDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@Id", mail.Id);

            connection.Open();
            command.ExecuteNonQuery();
            
            LogChange("Update", mail);
        }

        public void Delete(Guid id)
        {
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand("DELETE FROM Mails WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            command.ExecuteNonQuery();
            
            LogChange("Delete", new Mail { Id = id });
        }

        private Mail MapReaderToMail(SqlDataReader reader)
        {
            return new Mail
            {
                Id = Guid.Parse(reader["Id"].ToString()),
                MailType = reader["MailType"].ToString(),
                Description = reader["Description"].ToString(),
                RecipientId = Guid.Parse(reader["RecipientId"].ToString()),
                SenderId = Guid.Parse(reader["SenderId"].ToString()),
                Status = (reader["Status"].ToString()),
                ReceivedDate = DateTime.Parse(reader["ReceivedDate"].ToString()),
                ClaimedDate = reader["ClaimedDate"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(reader["ClaimedDate"].ToString()),
                ReceptionistId = Guid.Parse(reader["ReceptionistId"].ToString())
            };
        }
        
        private void LogChange(string operation, Mail mail)
        {
            var logMessage = $"{DateTime.Now}: {operation} - Mail ID: {mail.Id}";
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }
    }
}