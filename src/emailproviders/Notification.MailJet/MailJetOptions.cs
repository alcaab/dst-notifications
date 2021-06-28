namespace Desyco.Notification.MailJet
{
    public class MailJetOptions: EmailOptions
    {

        public MailJetOptions()
        {
        }

        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public string AttachmentDirectory { get; set; } 


    }
}