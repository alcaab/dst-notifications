using System;
using System.Linq;
using Desyco.Notification;


// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class NotificationServiceCollectionExtensions
    {
        public static IServiceCollection AddNotification(this IServiceCollection services,
            Action<NotificationOptions> configureOptions = null)
        {
            if (services.Any(x => x.ServiceType == typeof(NotificationOptions)))
                throw new InvalidOperationException("Notification services already registered");

            var options = new NotificationOptions(services);

            configureOptions?.Invoke(options);
            services.AddSingleton(options);

            //templating settings
            services.AddSingleton(options.TemplateEngineProvider);
            services.AddSingleton(options.TemplateCompilerProvider);
            services.AddSingleton(options.TemplateContentFactory);

            services.AddSingleton<IMemoryStorageProvider, MemoryProvider>();
            services.AddSingleton(options.WebSocketNotificationFactory);
            services.AddSingleton(options.EventHubFactory);
            services.AddTransient(options.StorageFactory);
            services.AddTransient(options.ExternalNotificationFactory);
            services.AddTransient<INotificationProvider, NotificationProvider>();
            services.AddSingleton<IBackgroundTask, NotificationServiceWorker>();
            services.AddSingleton<INotificationHost, NotificationHost>();

            
            return services;
        }


        public static NotificationOptions UseDefaultMailProvider(this NotificationOptions options, Action<NetMailOptions> configAction = null)
        {
            var config = new NetMailOptions();
            configAction?.Invoke(config);

            options.ConfigureProviderOptions(NotificationConst.ExternalProviderType,config);
            options.UseExternalProvider(sp => new NetMailProvider(
                sp.GetService<IStorageProvider>(),
                sp.GetService<INotificationEventHub>(),
                sp.GetService<NotificationOptions>(),
                sp.GetService<ITemplateContentProvider>()));

            return options;
        }
    }
}