using System;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public interface INotificationEventHub 
    {
        Task Emit(MessageEventArgs args);
        void Subscribe(Action<MessageEventArgs> subscription);

        Task Start();
        Task Stop();
    }
}