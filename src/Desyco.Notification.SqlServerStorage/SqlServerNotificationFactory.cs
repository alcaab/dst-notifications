using Desyco.Notification.EntityFramework;

namespace Desyco.Notification.SqlServerStorage
{
    public class SqlServerNotificationFactory : INotificationDbFactory
    {
        private readonly string _connectionString;

        public SqlServerNotificationFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public NotificationDbContext Build()
        {
            return new SqlServerNotificationDbContext(_connectionString);
        }
    }
}