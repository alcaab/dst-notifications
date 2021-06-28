using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Desyco.Notification;
using Desyco.Notification.Interfaces;
using Desyco.Notification.MailKitProvider;
using Desyco.Notification.Models;
using Desyco.Notification.Models.Events;
using Desyco.Notification.Models.Mail;
using Desyco.Notification.T5Templating;
using Desyco.NotificationService;
using Microsoft.Extensions.DependencyInjection;

namespace NotificationSample01
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider = ConfigureServices();

            //start the workflow host
            var host = serviceProvider.GetService<INotificationHost>();
            host.Start();

            var m = new NotificationMessage
            {
                NotificationMethod = NotificationMethod.Internal,
                Subject = "Prueba de envio de Notificacion",
                //Content = "Esta es una prueba de contenido del mesaje",
                To = new List<NotificationAddress>()
                {
                    new NotificationAddress("a.castro@mopc.gob.do", "alcaab", "Alexis Castro Abreu"),
                    new NotificationAddress("alcaab@gmail.com", "alcaab2", "Ronny Zapata Suero")
                },
                TemplateKey = "Prueba.t5",
                Data = new Dictionary<string, object>
                {
                    {"model", new DataModel{Nombre = "Alexis Castro Abreu"}}
                }
            };


            host.OnNotifyEvent += delegate (MessageEventArgs eventArgs)
            {
                switch (eventArgs.EventType)
                {
                    case NotificationEventType.Start:
                        Console.WriteLine($"Mensaje id {eventArgs.Message.Id} was Start");
                        break;
                    case NotificationEventType.Complete:
                        Console.WriteLine($"Mensaje id {eventArgs.Message.Id} was Complete");
                        break;
                    case NotificationEventType.Failed:
                        Console.WriteLine($"Mensaje id {eventArgs.Message.Id} was Fail, Attempts {eventArgs.Message.DeliveryAttempts}");
                        break;
                    case NotificationEventType.Sending:
                        Console.WriteLine($"Mensaje id {eventArgs.Message.Id} was Sending");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };


            await host.Notify(m);





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

                opt.MaxDeliveryAttempts = 5;
                opt.DeliveryAttemptsDelay = 60;
                opt.DisabledBackgroundServiceWorker = false;

                //opt.EnableSsl = false;
                //opt.NotificationMethod = NotificationMethod.External;
                //opt.UserName = "notificaciones@opdn.gov";
                //opt.Password = "KiramihsE19764";
                //opt.Address = "notificaciones@mopc.gob.do";
                //opt.DisplayName = "Notificaciones MOPC";
                //opt.HostName = "smtp.mopc.gob.do";
                //opt.Port = 25;


                //opt.EnableSsl = true;
                //opt.NotificationMethod = NotificationMethod.External;
                //opt.UserName = "notificaciones@mopc.gov.do";
                //opt.Password = "-!Lre4BT&Z~9";
                //opt.Address = "notificaciones@mopc.gov.do";
                //opt.DisplayName = "Notificaciones MOPC";
                //opt.HostName = "a2plcpnl0730.prod.iad2.secureserver.net";
                //opt.Port = 465;
                opt.UseMailKitProvider();

                opt.UseT5Templating();




            });
            //services.AddWorkflow(x => x.UseSqlServer(@"Server=.\SQLEXPRESS;Database=WorkflowCore;Trusted_Connection=True;", true, true));
            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }
    }
}



