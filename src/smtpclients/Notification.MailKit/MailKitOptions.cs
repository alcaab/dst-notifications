using MailKit.Net.Smtp;

namespace Desyco.Notification.MailKitProvider
{
    public class MailKitOptions: EmailOptions
    {
        public DeliveryStatusNotificationType DeliveryStatusNotificationType { get; set; } = DeliveryStatusNotificationType.Unspecified;
    }
}