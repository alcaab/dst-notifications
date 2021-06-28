namespace WebNotifier
{
    public class NotificationAddressModel
    {
        public string Address { get; set; }
        public string DisplayName { get; set; }
    }

    public class MessageModel
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string TemplateKey { get; set; } 
    }

    public enum RequestStatus
    {
        None,
        Submited,
        Assigned
    }
}
