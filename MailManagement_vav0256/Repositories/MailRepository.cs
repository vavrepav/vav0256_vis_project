using MailManagement_vav0256.Repositories.Interfaces;
using MailManagement_vav0256.Entities;
using Microsoft.Data.SqlClient;

namespace MailManagement_vav0256.Repositories
{
    public class MailRepository(string connectionString) : IMailRepository
    {
        private readonly string logFilePath = "MailRepository.txt";
        
        public IEnumerable<Mail> GetByStatus(string status)
    {
        var mails = new List<Mail>();
        using var connection = new SqlConnection(connectionString);
        var command = new SqlCommand(@"SELECT m.*, 
            s.Id AS SenderId, s.Name AS SenderName, s.ContactInfo AS SenderContactInfo,
            r.Id AS RecipientId, r.Email AS RecipientEmail, r.Role AS RecipientRole,
            rc.Id AS ReceptionistId, rc.Email AS ReceptionistEmail, rc.Role AS ReceptionistRole
            FROM Mails m
            LEFT JOIN Senders s ON m.SenderId = s.Id
            LEFT JOIN Users r ON m.RecipientId = r.Id
            LEFT JOIN Users rc ON m.ReceptionistId = rc.Id
            WHERE m.Status = @Status", connection);
        command.Parameters.AddWithValue("@Status", status);

        connection.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            mails.Add(MapReaderToMailWithDetails(reader));
        }
        return mails;
    }

    public IEnumerable<Mail> GetByRecipientIdAndStatus(Guid recipientId, string status)
    {
        var mails = new List<Mail>();
        using var connection = new SqlConnection(connectionString);
        var command = new SqlCommand(@"SELECT m.*, 
            s.Id AS SenderId, s.Name AS SenderName, s.ContactInfo AS SenderContactInfo,
            r.Id AS RecipientId, r.Email AS RecipientEmail, r.Role AS RecipientRole,
            rc.Id AS ReceptionistId, rc.Email AS ReceptionistEmail, rc.Role AS ReceptionistRole
            FROM Mails m
            LEFT JOIN Senders s ON m.SenderId = s.Id
            LEFT JOIN Users r ON m.RecipientId = r.Id
            LEFT JOIN Users rc ON m.ReceptionistId = rc.Id
            WHERE m.RecipientId = @RecipientId AND m.Status = @Status", connection);
        command.Parameters.AddWithValue("@RecipientId", recipientId);
        command.Parameters.AddWithValue("@Status", status);

        connection.Open();
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            mails.Add(MapReaderToMailWithDetails(reader));
        }
        return mails;
    }
        public IEnumerable<Mail> GetAll()
        {
            var mails = new List<Mail>();
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand(@"SELECT m.*, 
                s.Id AS SenderId, s.Name AS SenderName, s.ContactInfo AS SenderContactInfo,
                r.Id AS RecipientId, r.Email AS RecipientEmail, r.Role AS RecipientRole,
                rc.Id AS ReceptionistId, rc.Email AS ReceptionistEmail, rc.Role AS ReceptionistRole
                FROM Mails m
                LEFT JOIN Senders s ON m.SenderId = s.Id
                LEFT JOIN Users r ON m.RecipientId = r.Id
                LEFT JOIN Users rc ON m.ReceptionistId = rc.Id", connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                mails.Add(MapReaderToMailWithDetails(reader));
            }
            return mails;
        }

        public Mail GetById(Guid id)
        {
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand(@"SELECT m.*, 
                s.Id AS SenderId, s.Name AS SenderName, s.ContactInfo AS SenderContactInfo,
                r.Id AS RecipientId, r.Email AS RecipientEmail, r.Role AS RecipientRole,
                rc.Id AS ReceptionistId, rc.Email AS ReceptionistEmail, rc.Role AS ReceptionistRole
                FROM Mails m
                LEFT JOIN Senders s ON m.SenderId = s.Id
                LEFT JOIN Users r ON m.RecipientId = r.Id
                LEFT JOIN Users rc ON m.ReceptionistId = rc.Id
                WHERE m.Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapReaderToMailWithDetails(reader);
            }
            return null;
        }

        public IEnumerable<Mail> GetByRecipientId(Guid recipientId)
        {
            var mails = new List<Mail>();
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand(@"SELECT m.*, 
                s.Id AS SenderId, s.Name AS SenderName, s.ContactInfo AS SenderContactInfo,
                r.Id AS RecipientId, r.Email AS RecipientEmail, r.Role AS RecipientRole,
                rc.Id AS ReceptionistId, rc.Email AS ReceptionistEmail, rc.Role AS ReceptionistRole
                FROM Mails m
                LEFT JOIN Senders s ON m.SenderId = s.Id
                LEFT JOIN Users r ON m.RecipientId = r.Id
                LEFT JOIN Users rc ON m.ReceptionistId = rc.Id
                WHERE m.RecipientId = @RecipientId", connection);
            command.Parameters.AddWithValue("@RecipientId", recipientId);

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                mails.Add(MapReaderToMailWithDetails(reader));
            }
            return mails;
        }

        public Mail Create(Mail mail, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var closeConnection = false;
            if (connection == null)
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                transaction = connection.BeginTransaction();
                closeConnection = true;
            }

            var command = new SqlCommand(@"INSERT INTO Mails 
        (Id, MailType, Description, RecipientId, SenderId, Status, ReceivedDate, ClaimedDate, ReceptionistId) 
        VALUES (@Id, @MailType, @Description, @RecipientId, @SenderId, @Status, @ReceivedDate, @ClaimedDate, @ReceptionistId)", connection, transaction);
            command.Parameters.AddWithValue("@Id", mail.Id);
            command.Parameters.AddWithValue("@MailType", mail.MailType);
            command.Parameters.AddWithValue("@Description", mail.Description);
            command.Parameters.AddWithValue("@RecipientId", mail.RecipientId);
            command.Parameters.AddWithValue("@SenderId", mail.SenderId);
            command.Parameters.AddWithValue("@Status", mail.Status);
            command.Parameters.AddWithValue("@ReceivedDate", mail.ReceivedDate);
            command.Parameters.AddWithValue("@ClaimedDate", (object)mail.ClaimedDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@ReceptionistId", mail.ReceptionistId);

            command.ExecuteNonQuery();

            LogChange("Create", mail);

            if (!closeConnection) return mail;
            transaction.Commit();
            connection.Close();

            return mail;
        }

        public void Update(Mail mail, SqlConnection connection = null, SqlTransaction transaction = null)
        {
            var closeConnection = false;
            if (connection == null)
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                transaction = connection.BeginTransaction();
                closeConnection = true;
            }

            var command = new SqlCommand("UPDATE Mails SET MailType = @MailType, Description = @Description, RecipientId = @RecipientId, SenderId = @SenderId, Status = @Status, ClaimedDate = @ClaimedDate WHERE Id = @Id", connection, transaction);
            command.Parameters.AddWithValue("@MailType", mail.MailType);
            command.Parameters.AddWithValue("@Description", mail.Description);
            command.Parameters.AddWithValue("@RecipientId", mail.RecipientId);
            command.Parameters.AddWithValue("@SenderId", mail.SenderId);
            command.Parameters.AddWithValue("@Status", mail.Status);
            command.Parameters.AddWithValue("@ClaimedDate", (object)mail.ClaimedDate ?? DBNull.Value);
            command.Parameters.AddWithValue("@Id", mail.Id);

            command.ExecuteNonQuery();

            LogChange("Update", mail);

            if (!closeConnection) return;
            transaction.Commit();
            connection.Close();
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
        
        private Mail MapReaderToMailWithDetails(SqlDataReader reader)
        {
            var mail = new Mail
            {
                Id = Guid.Parse(reader["Id"].ToString()),
                MailType = reader["MailType"].ToString(),
                Description = reader["Description"].ToString(),
                RecipientId = Guid.Parse(reader["RecipientId"].ToString()),
                SenderId = Guid.Parse(reader["SenderId"].ToString()),
                Status = reader["Status"].ToString(),
                ReceivedDate = DateTime.Parse(reader["ReceivedDate"].ToString()),
                ClaimedDate = reader["ClaimedDate"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(reader["ClaimedDate"].ToString()),
                ReceptionistId = Guid.Parse(reader["ReceptionistId"].ToString())
            };

            mail.Sender = new Sender
            {
                Id = reader["SenderId"] == DBNull.Value ? Guid.Empty : Guid.Parse(reader["SenderId"].ToString()),
                Name = reader["SenderName"] == DBNull.Value ? null : reader["SenderName"].ToString(),
                ContactInfo = reader["SenderContactInfo"] == DBNull.Value ? null : reader["SenderContactInfo"].ToString()
            };

            mail.Recipient = new User
            {
                Id = reader["RecipientId"] == DBNull.Value ? Guid.Empty : Guid.Parse(reader["RecipientId"].ToString()),
                Email = reader["RecipientEmail"] == DBNull.Value ? null : reader["RecipientEmail"].ToString(),
                Role = reader["RecipientRole"] == DBNull.Value ? null : reader["RecipientRole"].ToString()
            };

            mail.Receptionist = new User
            {
                Id = reader["ReceptionistId"] == DBNull.Value ? Guid.Empty : Guid.Parse(reader["ReceptionistId"].ToString()),
                Email = reader["ReceptionistEmail"] == DBNull.Value ? null : reader["ReceptionistEmail"].ToString(),
                Role = reader["ReceptionistRole"] == DBNull.Value ? null : reader["ReceptionistRole"].ToString()
            };

            return mail;
        }
        
        private void LogChange(string operation, Mail mail)
        {
            var logMessage = $"{DateTime.Now}: {operation} - Mail ID: {mail.Id}";
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        }
    }
}