using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Desyco.Notification.Templating;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public class NotificationHost : INotificationHost, IDisposable
    {
        private readonly INotificationEventHub _eventHub;
        private readonly INotificationProvider _notificationProvider;
        private readonly ITemplateCompilerService _templateCompiler;
        private readonly IEnumerable<IBackgroundTask> _backgroundTasks;

        private bool _turnoff = true;

        public NotificationHost(
            NotificationOptions options,
            ILoggerFactory loggerFactory,
            INotificationProvider notificationProvider,
            INotificationEventHub eventHub,
            ITemplateCompilerService templateCompiler,
            IStorageProvider storageProvider,
            IEnumerable<IBackgroundTask> backgroundTasks
            )
        {


            StorageProvider = storageProvider;
            Logger = loggerFactory.CreateLogger<NotificationHost>();
            Options = options;
            _notificationProvider = notificationProvider;
            _eventHub = eventHub;
            _templateCompiler = templateCompiler;
            _backgroundTasks = backgroundTasks;
            _eventHub.Subscribe(NotifyEventHandle);
        }


        public void Dispose()
        {
            if (!_turnoff)
                Stop();
        }

        public ILogger Logger { get; private set; }

        public event NotificationEventHandler OnNotifyEvent;

        public NotificationOptions Options { get; private set; }

        public Task<string> Notify(NotificationMessage m)
        {
            //para evitar el envio directo desde otro lugar que no sea el host.
            //desde fuera de la libreria;
            m.AllowSend = true;
            m.CreatedDate = DateTime.UtcNow;
            m.Group = Guid.NewGuid().ToString();
            //solo configura en el primer intento

            _ = Task.Run(async () =>
            {
                await _notificationProvider.Notify(m);
            });

            return Task.FromResult(m.Group);
        }


        public IStorageProvider StorageProvider { get; }


        public void Start()
        {
            StartAsync(CancellationToken.None).Wait();
        }

        public void Stop()
        {
            StopAsync(CancellationToken.None).Wait();
        }

        protected void NotifyEventHandle(MessageEventArgs evt)
        {
            OnNotifyEvent?.Invoke(evt);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _turnoff = false;

            StorageProvider.EnsureStoreExists();

            await _eventHub.Start();
            await _templateCompiler.Start();

            foreach (var backgroundTask in _backgroundTasks)
                backgroundTask.Start();

            Logger.LogInformation("Iniciando tareas en segundo plano");

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _turnoff = true;
            Logger.LogInformation("Deteniendo tareas en segundo plano");

            foreach (var backgroundTask in _backgroundTasks)
                backgroundTask.Stop();

            Logger.LogInformation("Tareas del trabajador detenidas");

            await _eventHub.Stop();
            await _templateCompiler.Stop();

        }
    }
}