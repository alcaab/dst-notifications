namespace Desyco.Notification.Services
{
    public interface IDeliveryStrategy
    {
        MessageContainer GetNotifications(PlainMessage message);     
    }
}
