// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public class RecipientInfo 
    {
        public RecipientInfo()
        {

        }
        public RecipientInfo(string address, string userName, string name,  NotificationMethod method )
        { 
            Name = name;
            Address = address;
            UserName = userName;
            NotificationMethod = method;
        }
        public RecipientInfo(string address, string name) : this(address, address, name, NotificationMethod.Inherited)
        {

        }

        public RecipientInfo(string address, string name, NotificationMethod method) : this(address, address, name, method)
        {

        }

        public string UserName { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        public NotificationMethod NotificationMethod { get; set; } = NotificationMethod.Inherited;

    }
}