using MailManagement_vav0256.Repositories.Interfaces;
using MailManagement_vav0256.Entities;
using Microsoft.Data.SqlClient;

namespace MailManagement_vav0256.Repositories
{
    public class SenderRepository(string connectionString) : ISenderRepository
    {
        public IEnumerable<Sender> GetAll()
        {
            var senders = new List<Sender>();
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand("SELECT * FROM Senders", connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                senders.Add(MapReaderToSender(reader));
            }
            return senders;
        }

        public Sender GetById(Guid id)
        {
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand("SELECT * FROM Senders WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapReaderToSender(reader);
            }
            return null;
        }

        public Sender Create(Sender sender)
        {
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand("INSERT INTO Senders (Id, Name, ContactInfo) VALUES (@Id, @Name, @ContactInfo)", connection);
            command.Parameters.AddWithValue("@Id", sender.Id);
            command.Parameters.AddWithValue("@Name", sender.Name);
            command.Parameters.AddWithValue("@ContactInfo", sender.ContactInfo);

            connection.Open();
            command.ExecuteNonQuery();

            return sender;
        }

        public void Update(Sender sender)
        {
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand("UPDATE Senders SET Name = @Name, ContactInfo = @ContactInfo WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Name", sender.Name);
            command.Parameters.AddWithValue("@ContactInfo", sender.ContactInfo);
            command.Parameters.AddWithValue("@Id", sender.Id);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void Delete(Guid id)
        {
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand("DELETE FROM Senders WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            command.ExecuteNonQuery();
        }

        private Sender MapReaderToSender(SqlDataReader reader)
        {
            return new Sender
            {
                Id = Guid.Parse(reader["Id"].ToString()),
                Name = reader["Name"].ToString(),
                ContactInfo = reader["ContactInfo"].ToString()
            };
        }
    }
}