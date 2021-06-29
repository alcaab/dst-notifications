
using System;
// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public class NotificationAddress 
    {
        public NotificationAddress()
        {

        }

        public NotificationAddress(string address, string userName, string name)
        {
            DisplayName = name;
            Address = address;
            UserName = userName;
        }

        public NotificationAddress(string address, string name):this(address,null, name)
        {
 
        }


        public string UserName { get; set; }
        public string Address { get; set; }
        public string DisplayName { get; set; }
        public string Body { get; set; }

        [Obsolete("Use NotificationSubject insted",true)]
        public string TemplateKey { get; set; }
        [Obsolete("Use NotificationSubject insted", true)]
        public NotificationMethod NotificationMethod { get; set; }
    }
}