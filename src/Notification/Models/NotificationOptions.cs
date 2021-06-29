using System;
using System.Collections.Generic;
using Desyco.Notification.Services;
using Desyco.Notification.Services.Default;
using Desyco.Notification.Templating;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{

    public class NotificationOptions
    {
        internal Func<IServiceProvider, INotificationEventHub> EventHubFactory;
        internal Func<IServiceProvider, IExternalNotificationProvider> ExternalNotificationFactory;
        internal Func<IServiceProvider, IStorageProvider> StorageFactory;
        internal Func<IServiceProvider, ITemplateCompilerService> TemplateCompilerProvider;
        internal Func<IServiceProvider, ITemplateEngine> TemplateEngineProvider;
        internal Func<IServiceProvider, ITemplateContentProvider> TemplateContentFactory;
        internal Func<IServiceProvider, IInternalNotificationProvider> WebSocketNotificationFactory;
        internal Func<IServiceProvider, IDeliveryStrategy> DeliveryStrategy; 

        public NotificationOptions(IServiceCollection services) 
        {
            Services = services;

            //templating settings
            TemplateEngineProvider = sp => new DefaultTemplateEngine();
            TemplateCompilerProvider = sp => new DefaultTemplateCompilerService(sp.GetService<ITemplateEngine>());
            TemplateContentFactory = sp => new DefaultTemplateContentProvider();

            //others
            StorageFactory = sp => new DefaultStorageProvider(sp.GetService<IMemoryStorageProvider>());

            WebSocketNotificationFactory = sp => new DefaultInternalNotificationProvider(
                sp.GetService<IStorageProvider>(),
                sp.GetService<INotificationEventHub>(),
                sp.GetService<NotificationOptions>(),
                sp.GetService<ITemplateContentProvider>());

            ExternalNotificationFactory = sp => new DefaultExternalNotificationProvider(
                sp.GetService<IStorageProvider>(),
                sp.GetService<INotificationEventHub>(),
                sp.GetService<NotificationOptions>(),
                sp.GetService<ITemplateContentProvider>(),
                sp.GetService<ILoggerFactory>());

            DeliveryStrategy = sp => new DefaultDeliveryStrategy(sp.GetService<NotificationOptions>());
            EventHubFactory = sp => new NotificationEventHub();
            Events = new NotificationEvents();
            ProviderOptions = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        }

        public IServiceCollection Services { get; set; }
        public string ServiceId { get; set; }


        /// <summary>
        ///     Intervalo de tiempo en minutos que el servicio tardara para  reeviar las notificaciones
        ///     fallidas
        /// </summary>
        public int DeliveryAttemptsDelay { get; set; } = 5;

        /// <summary>
        /// No está implementado aún
        /// </summary>
        public int? StoredNotificationsExpirationDays { get; set; } = 15;

        /// <summary>
        ///     Cantidad máxima de intentos que el servicio hará para reenviar las notificaciones fallidas
        /// </summary>
        public int MaxDeliveryAttempts { get; set; }

        public bool DisabledForwardingServiceWorker { get; set; } 

        public NotificationEvents Events { get; set; }

        public void UseStorage(Func<IServiceProvider, IStorageProvider> factory)
        {
            StorageFactory = factory;
        }

        public void UseExternalProvider(Func<IServiceProvider, IExternalNotificationProvider> externalProvider)
        {
            ExternalNotificationFactory = externalProvider;
        }

        public void UseWebSocket(Func<IServiceProvider, IInternalNotificationProvider> webSocketProvider)
        {
            WebSocketNotificationFactory = webSocketProvider;
        }

        public void UseTemplateCompiler(Func<IServiceProvider, ITemplateEngine> engineProvider,
            Func<IServiceProvider, ITemplateCompilerService> compilerProvider)
        {
            TemplateEngineProvider = engineProvider;
            TemplateCompilerProvider = compilerProvider;
        }

        public void UseTemplateContent(Func<IServiceProvider, ITemplateContentProvider> contentProvider)
        {
            if (contentProvider != null)
                TemplateContentFactory = contentProvider;
        }

        private Dictionary<string, object> ProviderOptions { get; set; }

        public void ConfigureProviderOptions(string providerType,object optionsProvider)
        {
            if (ProviderOptions.ContainsKey(providerType))
                ProviderOptions[providerType] = optionsProvider;
            else
                ProviderOptions.Add(providerType, optionsProvider);
        }

        public T GetExternalOptionsProvider<T>(string providerType)
        {

            if (ProviderOptions.Count == 0 || typeof(T) != ProviderOptions[providerType].GetType())
                throw new Exception("Invalid operation.");

            return (T)ProviderOptions[providerType];

        }


        public void UseDeliveryStrategy(Func<IServiceProvider, IDeliveryStrategy> deliveryStrategy)
        {
            DeliveryStrategy = deliveryStrategy;
        }

        //public void UseAppSettingsOptions(string seccionKey = "")
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json", true, true)
        //        .Build();

        //    if (!string.IsNullOrEmpty(seccionKey))
        //    {
        //        builder.GetSection(seccionKey).Bind(this);
        //        return;
        //    }

        //    builder.Bind(this);
        //}

    }
}

