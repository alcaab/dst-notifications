using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Desyco.Notification;
using Desyco.Notification.EntityFramework;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static NotificationOptions UseEntityFrameworkStorage(
            this NotificationOptions options,
            INotificationDbFactory dbFactory, bool createDb, bool migrateDb)
        {

            options
                .UseStorage(sp =>
                    new EntityFrameworkStorageProvider(dbFactory,
                        sp.GetService<NotificationOptions>(),
                        sp.GetService<IMapper>(), createDb, migrateDb));

            if (options.Services != null)
            {

                var mappingConfig = new MapperConfiguration(cfg =>
                {
                    cfg.AddCollectionMappers();
                    cfg.AddProfile(new MappingProfile());
                });

                options.Services.AddSingleton(mappingConfig.CreateMapper());
            }


            return options;
        }
    }
}