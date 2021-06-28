using Microsoft.Extensions.Options;

namespace Desyco.Notification
{
    public class NotificationPostConfigureOptions :  IPostConfigureOptions<NotificationOptions>
    {
        public void PostConfigure(string name, NotificationOptions options)
        {

        }
    }
}
