using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification.EntityFramework
{
    public class NotificationConfigurationProvider : ConfigurationProvider
    {
        private readonly NotificationConfigurationSource _source;

        public NotificationConfigurationProvider(NotificationConfigurationSource source)
        {
            _source = source;
        }

        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<NotificationDbContext>();
            _source.Options(builder);
        }

    }
}