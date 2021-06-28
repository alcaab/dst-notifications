using System;
using Desyco.Notification;
using Desyco.Notification.SignalRProvider.Hub;
using Desyco.Notification.SignalRProvider.Interface;
using Desyco.Notification.SignalRProvider.Services;
using Microsoft.AspNetCore.SignalR;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    public static class NotificationHubServiceCollectionExtensions
    {
        public static ISignalRServerBuilder AddSignalR(this NotificationOptions options,
            Action<HubOptions> configure = null)
        {
            var signalR = configure == null 
                ? options.Services.AddSignalR() 
                : options.Services.AddSignalR(configure);

            options.UseWebSocket(sp =>
                new SignalRNotificationProvider(
                    sp.GetService<IStorageProvider>(),
                    sp.GetService<INotificationEventHub>(),
                    sp.GetService<NotificationOptions>(),
                    sp.GetService<ITemplateContentProvider>(),
                    sp.GetService<IHubContext<NotificationHub, INotificationHub>>()));


            return signalR;
        }


        //public static IApplicationBuilder UseSignalR(this WebSocketRouteBuilder options,
        //    Action<HubRouteBuilder> configure)
        //{
        //    return options.ApplicationBuilder.UseSignalR(configure);
        //}


        //public static IApplicationBuilder UseSignalRHub(this WebSocketRouteBuilder configuration)
        //{
        //    var configService = configuration.ApplicationBuilder.ApplicationServices
        //        .GetRequiredService<NotifierOptions>();

        //    configuration.Path = GetHub(configService.ListenPath);
        //    //var path  = GetHub(configService.ListenPath);

        //    void Configure(HubRouteBuilder cfg)
        //    {
        //        cfg.MapHub<NotificationHub>(configuration.Path);
        //    }

        //    configuration.ApplicationBuilder.UseSignalR(Configure);

        //    return configuration.ApplicationBuilder;
        //}

        //private static PathString GetHub(string path)
        //{
        //    //var pos = path.IndexOf(']');
        //    //if (pos == -1)
        //    //    throw new Exception("Specify the segment of the websock that you want to hear in brackets[].example: [/notifications]/messenger");

        //    //return path.Substring(1, pos - 1);
        //    return path;
        //}
    }
}