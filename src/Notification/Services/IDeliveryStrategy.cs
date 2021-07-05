namespace Desyco.Notification.Services
{
    public interface IDeliveryStrategy
    {
        NotificationContainer GetNotifications(PlainMessage message);     
    }
}
