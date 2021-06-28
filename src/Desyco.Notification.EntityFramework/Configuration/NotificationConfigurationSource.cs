using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification.EntityFramework
{
    public class NotificationConfigurationSource : IConfigurationSource
    {
        public Action<DbContextOptionsBuilder> Options { get; set; }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new NotificationConfigurationProvider(this);
        }

    }
}
