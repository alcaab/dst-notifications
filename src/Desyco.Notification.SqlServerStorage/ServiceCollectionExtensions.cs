using Desyco.Notification;
using Desyco.Notification.SqlServerStorage;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static NotificationOptions UseSqlServer(
            this NotificationOptions options,
            string connectionString, bool ensureDbExists)
        {
            options
                .UseEntityFrameworkStorage(
                    new SqlServerNotificationFactory(connectionString), ensureDbExists, false);

            return options;
        }
    }
}