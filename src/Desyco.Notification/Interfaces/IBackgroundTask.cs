// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public interface IBackgroundTask
    {
        void Start();
        void Stop();
    }
}