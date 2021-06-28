using System;
using System.Collections.Generic;
using System.IO;
using Desyco.Notification;
using Desyco.Notification.T5Templating;
using Microsoft.Extensions.DependencyInjection;

namespace NetFramework461
{
    class Program
    {
        static void Main(string[] args)
        {

            var serviceProvider = ConfigureServices();

            var host = serviceProvider.GetService<INotificationHost>();

            host.Start();

            host.OnNotifyEvent += async delegate (MessageEventArgs eventArgs)
            {
                switch (eventArgs.EventType)
                {
                    case NotificationEventType.Start:
                        await host.Options.Events.NotificationStart(eventArgs as MessageStart);
                        break;
                    case NotificationEventType.Sending:
                        await host.Options.Events.NotificationSending(eventArgs as MessageSending);
                        break;
                    case NotificationEventType.Delivered:
                        await host.Options.Events.NotificationDelivered(eventArgs as MessageDelivered);
                        break;
                    case NotificationEventType.Failed:
                        await host.Options.Events.NotificationFailed(eventArgs as MessageFailed);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };


            var m = new NotificationMessage
            {
                NotificationMethod = NotificationMethod.External,
                Subject = "Prueba de envio de Notificacion",
                Body = "Esta es una prueba de contenido del mesaje",
                To = new List<NotificationAddress>()
                {
                    new NotificationAddress("sample@gmail.com", "Alexis Castro")
                },
                //Ony if content is null templating will do its job
                TemplateKey = "Prueba.t5"
            };

            host.Notify(m);

            Console.ReadLine();

            host.Stop();
        }

        private static IServiceProvider ConfigureServices()
        {
            //setup dependency injection
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddNotification(opt =>
            {
                opt.UseMailKitProvider(cfg =>
                {
                    cfg.FromAddress = "sample@gmail.com";
                    cfg.SenderName = "Notification Service";

                    cfg.SmtpServerProfiles.Add(new SmtpParameters
                    {
                        EnableSsl = true,
                        UserName = "sample@gmail.com",
                        Password = "*********",
                        SmtpServer = "smtp.gmail.com",
                        Port = 465
                    });
                });
                opt.UseT5FileTemplating(Path.Combine(Directory.GetCurrentDirectory(),"templates") );
                //opt.AddSignalR(hubOptions => { hubOptions.EnableDetailedErrors = true; });
            });

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
