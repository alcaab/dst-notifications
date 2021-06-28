using System;
using System.Linq;
using System.Threading.Tasks;


// ReSharper disable once CheckNamespace
namespace Desyco.Notification
{
    public abstract class EmailNotificationProvider<TOptions> : ExternalNotificationProvider
        where TOptions : EmailOptions
    {
        private readonly TOptions _options;


        protected EmailNotificationProvider(
            IStorageProvider storageProvider, 
            INotificationEventHub eventHub,
            NotificationOptions options, 
            ITemplateContentProvider templateContent
            ) : base(storageProvider, eventHub, options, templateContent)
        {
            _options = options.GetExternalOptionsProvider<TOptions>(NotificationConst.ExternalProviderType);
        }

        protected override async Task SendNotificationAsync(NotificationMessage m)
        {

            var emailOptions = _options as EmailOptions ?? throw new Exception();


            foreach (var opts in emailOptions.SmtpServerProfiles)
            {
                try
                {
                    if (m.From == null)
                        m.From = new NotificationAddress
                        {
                            DisplayName = emailOptions.SenderName,
                            Address = emailOptions.FromAddress
                        };

                    await TrySendNotificationAsync(opts, m);
                    //sale la primera vez que sea exitoso
                    break;
                }
                catch
                {
                    if (opts.Equals(emailOptions.SmtpServerProfiles.Last()))
                        throw;
                    //Logger
                }
            }


        }

        protected abstract Task TrySendNotificationAsync(SmtpParameters parameters, NotificationMessage m);


    }
}