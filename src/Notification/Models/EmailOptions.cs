using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public class EmailOptions
    {
        public EmailOptions()
        {
            SmtpServerProfiles = new List<SmtpParameters>();
        }
        public string FromAddress { get; set; }
        public string SenderName { get; set; }
        public List<SmtpParameters> SmtpServerProfiles { get; set; }

    }


}