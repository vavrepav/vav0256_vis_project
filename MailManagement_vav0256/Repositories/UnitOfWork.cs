using Microsoft.Data.SqlClient;

namespace MailManagement_vav0256.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private readonly SqlConnection _connection;
        private readonly SqlTransaction _transaction;
        private bool _disposed;

        public UnitOfWork(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public SqlConnection Connection => _connection;
        public SqlTransaction Transaction => _transaction;

        public void Commit()
        {
            try
            {
                _transaction?.Commit();
            }
            catch
            {
                _transaction?.Rollback();
                throw;
            }
            finally
            {
                Dispose();
            }
        }

        public void Rollback()
        {
            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _transaction?.Dispose();
                _connection?.Close();
                _connection?.Dispose();
                _disposed = true;
            }
        }
    }
}