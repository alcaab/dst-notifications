using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    internal class NotificationServiceWorker : BackgroundService, IBackgroundTask
    {
        private readonly IStorageProvider _dataProvider;

        private readonly ILogger _logger;
        private readonly NotificationOptions _options;
        private readonly INotificationProvider _provider;

        public NotificationServiceWorker(
            INotificationProvider provider,
            IStorageProvider dataProvider,
            NotificationOptions options,
            ILoggerFactory loggerFactory
           )
        {
            _provider = provider;
            _dataProvider = dataProvider;
            _options = options;
            _logger = loggerFactory.CreateLogger<NotificationServiceWorker>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _logger.LogDebug("Notification Service is starting.");

            stoppingToken.Register(() =>
                _logger.LogDebug(" Notification task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                if (!_options.DisabledForwardingServiceWorker)
                {
                    var messages = await _dataProvider.GetFailedNotificationsAsync();
                    foreach (var m in messages)
                    {
                        m.AllowSend = true;
                        await _provider.Notify(m);
                    }
                }
                await Task.Delay(TimeSpan.FromMinutes(_options.DeliveryAttemptsDelay), stoppingToken);
            }
            _logger.LogDebug("Notification task is stopping.");
        }

        public void Start()
        {
            StartAsync(CancellationToken.None).Wait();
        }

        public void Stop()
        {
            StopAsync(CancellationToken.None).Wait();
        }
    }
}