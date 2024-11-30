using MailManagement_vav0256.Repositories.Interfaces;
using MailManagement_vav0256.Entities;
using Microsoft.Data.SqlClient;

namespace MailManagement_vav0256.Repositories
{
    public class UserRepository(string connectionString) : IUserRepository
    {
        public User GetByEmailAndPassword(string email, string password)
        {
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand("SELECT * FROM Users WHERE Email = @Email AND Password = @Password", connection);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Password", password);

            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapReaderToUser(reader);
            }
            return null;
        }

        public IEnumerable<User> GetAll()
        {
            var users = new List<User>();
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand("SELECT * FROM Users", connection);

            connection.Open();
            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                users.Add(MapReaderToUser(reader));
            }
            return users;
        }

        public User GetById(Guid id)
        {
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand("SELECT * FROM Users WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapReaderToUser(reader);
            }
            return null;
        }

        public User Create(User user)
        {
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand("INSERT INTO Users (Id, Email, Password, Role) VALUES (@Id, @Email, @Password, @Role)", connection);
            command.Parameters.AddWithValue("@Id", user.Id);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Password", user.Password);
            command.Parameters.AddWithValue("@Role", user.Role);

            connection.Open();
            command.ExecuteNonQuery();

            return user;
        }

        public void Update(User user)
        {
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand("UPDATE Users SET Email = @Email, Password = @Password, Role = @Role WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Password", user.Password);
            command.Parameters.AddWithValue("@Role", user.Role);
            command.Parameters.AddWithValue("@Id", user.Id);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void Delete(Guid id)
        {
            using var connection = new SqlConnection(connectionString);
            var command = new SqlCommand("DELETE FROM Users WHERE Id = @Id", connection);
            command.Parameters.AddWithValue("@Id", id);

            connection.Open();
            command.ExecuteNonQuery();
        }

        private User MapReaderToUser(SqlDataReader reader)
        {
            return new User
            {
                Id = Guid.Parse(reader["Id"].ToString()),
                Email = reader["Email"].ToString(),
                Password = reader["Password"].ToString(),
                Role = reader["Role"].ToString()
            };
        }
    }
}