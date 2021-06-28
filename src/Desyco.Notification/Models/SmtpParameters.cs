



// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public class SmtpParameters
    {
        public bool EnableSsl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }

    }

    //public class HostParametersBase
    //{
    //    public Dictionary<string,object> Props { get; set; }
    //}
}