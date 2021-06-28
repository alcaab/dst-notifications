# Desyco.Notification

 *Desyco.Notification* is a small .Net Standard library designed to easily add notification functionality to your .Net Web API application. It relies on existing smtp clients and websocket libraries to deliver notification via emails, SMS services and/or WebSocket protocol.
 

## Main features
* Keeps a record of all sent notifications due to it implements persistence storage providers. It currently supports Memory (for testing only) and MsSqlServer Database persistence but it's designed to easily implement any other data source provider.

* Contains a Background Service Worker (NotificationServiceWorker) which is responsible for forwarding notifications that for some reason could not be delivered. Through configuration it's possible to set the number of attempts and the interval between one attempt and another and event disable it.

* Support for text templating. It currently uses a custom implementation of [T5.Template Library](https://github.com/atifaziz/t5) to which I made some slight modifications to adapt it for the specific purposes of *Desyco.Notification*. By default, the templates are read from the file system but it is possible to implement extensions to save and read them from a data persistence storage.


* Multiple Smtp servers. It's posible to registrer N number of smtp servers and they work as follows: when a notification is sent and the first server is not available it goes to the next and tries to deliver the message, this continues until the message is delivered or list of servers has been exhausted.

* Contains an event hub which exposes the status of messages throughout its life cycle. At the moment message life cycle contains 4 events:

    * MessageStart: Message is been preparing to be sent
    * MessageSending: Message is being sent.
    * MessageDelivered: Message was successfully delivered
    * MessageFailed: Message could not be delivered

## Setup
Use the AddNotification extension method for IServiceCollection to setup the notification service upon ConfigureServices method at startup class of your application. By default, external notifications are delivered with [System.Net.Mail](https://docs.microsoft.com/en-us/dotnet/api/system.net.mail) library using the *UseDefaultMailProvider* extension method.

```c#
services.AddNotification(opt =>
{
    //Background Worker forwarding service configuration
    opt.DisabledForwardingServiceWorker = false;
    opt.DeliveryAttemptsDelay = 10;
    opt.MaxDeliveryAttempts = 3;    

    opt.UseDefaultMailProvider(cfg =>
    {
        cfg.FromAddress = "sample@gmail.com";
        cfg.SenderName = "Notification Service";
        cfg.UseDefaultCredentials = false;
        cfg.SmtpServerProfiles.Add(new SmtpParameters
        {
            EnableSsl = true,
            UserName = "sample@gmail.com",
            Password = "******",
            SmtpServer = "smtp.gmail.com",
            Port = 587
        });
    });

});
```

You can also configure other external notification provider. There are somes emails providers extensions implementation as separated libraries.

- *Desyco.Notification.MailKit* Implementation for [Mailkit](https://es.mailjet.com) smtp client API
- *Desyco.Notification.MailJet* Implementation for [MailJet API](https://es.mailjet.com)
- *Desyco.Notification.SendGridProvider* Implementation for [Twilio SendGrid API](https://sendgrid.com/)


By calling IApplicationBuilder extension method *UseNotification* you will start all needed services so library could do its job.

```C#
 app.UseNotification();
```
## Usage
Once configuration is completed you can inject *INotificationHost* into your services/controllers:

```c#
using System.Collections.Generic;
using System.Threading.Tasks;
using Desyco.Notification;
using Microsoft.AspNetCore.Mvc;

namespace WebNotifier.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly INotificationHost _notificationHost;

        public HomeController(INotificationHost notificationHost)
        {
            _notificationHost = notificationHost;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> NotifyMe(MessageModel model)
        {
            var message = new NotificationMessage
            {
                //
                NotificationMethod = NotificationMethod.External,
                Subject = model.Subject,
                Body = model.Body,
                TemplateKey = model.TemplateKey,
                To = new List<NotificationAddress>(){ new NotificationAddress(model.Email, model.DisplayName) }
            };

            await _notificationHost.Notify(message);

            return Ok(message.Group);
        }
    }

    public class MessageModel
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string TemplateKey { get; set; } 
    }
}
```
Probably, you will need to subscribe some events to perform some tasks before or after notifications are sent. To achieve this, *Desyco.Notification* provides an EventHub service that makes it possible.

```c#
services.AddNotification(opt =>
{
    ......

    opt.Events = new NotificationEvents
    {
        OnNotificationStart = arg =>
        {
            Console.WriteLine($"Message id {arg.Message.Group} is ready to be sent");
            return Task.CompletedTask;
        },

        OnNotificationSending = arg =>
        {
            Console.WriteLine($"Message id {arg.Message.Group} sending");
            return Task.CompletedTask;
        },

        OnNotificationDelivered = arg =>
        {
            Console.WriteLine($"Message id {arg.Message.Group} delivered.");
            return Task.CompletedTask;
        },

        OnNotificationFailed = arg =>
        {
            Console.WriteLine($"Message id {arg.Message.Group} failed.");
            return Task.CompletedTask;
        }
    };    

});
```
