// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public class RecipientInfo 
    {
        public RecipientInfo()
        {

        }
        public RecipientInfo(string address, string userName, string name)
        { 
            Name = name;
            Address = address;
            UserName = userName;
        }
        public RecipientInfo(string address, string name) : this(address, null, name)
        {

        }

        public string UserName { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }

    }
}