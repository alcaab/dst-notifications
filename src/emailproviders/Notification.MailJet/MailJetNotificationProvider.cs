using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;

namespace Desyco.Notification.MailJet
{
    public class MailJetNotificationProvider : EmailNotificationProvider<MailJetOptions>
    {
        private readonly MailJetOptions _options;


        public MailJetNotificationProvider(
            IStorageProvider storageProvider,
            INotificationEventHub eventHub,
            NotificationOptions options,
            ITemplateContentProvider templateContent) : base(storageProvider, eventHub, options, templateContent)
        {
            _options = options.GetExternalOptionsProvider<MailJetOptions>(NotificationConst.ExternalProviderType);
        }

        protected override async Task TrySendNotificationAsync(SmtpParameters parameters, NotificationMessage m)
        {

            var client = new MailjetClient(_options.ApiKey, _options.ApiSecret)
            {
                Version = ApiVersion.V3_1,
            };

            var response = await client.PostAsync(PrepareMessage(m));
            if (!response.IsSuccessStatusCode)
            {
                //var er = response.GetErrorMessage();
                throw new Exception(response.GetErrorMessage());
            }

        }

        private MailjetRequest PrepareMessage(NotificationMessage m)
        {

            var msg = new MailjetRequest
            {
                Resource = Send.Resource
            };

            var tos = new JArray();
            m.To.ForEach(t =>
            {
                tos.Add(new JObject
                {
                    {"Email", t.Address},
                    {"Name", t.DisplayName}
                });
            });


            var bccs = new JArray();
            m.Bcc.ForEach(t =>
            {
                bccs.Add(new JObject
                {
                    {"Email", t.Address},
                    {"Name", t.DisplayName}
                });
            });

            var ccs = new JArray();
            m.Cc.ForEach(t =>
            {
                ccs.Add(new JObject
                {
                    {"Email", t.Address},
                    {"Name", t.DisplayName}
                });
            });

            var attachments = new JArray();
            m.Attachments.ForEach(att =>
            {
                var fileName = Path.GetFileName(att.FileName);
                var fullPath = att.FileName;

                if (!File.Exists(fullPath))
                {
                    fullPath = Path.Combine(_options.AttachmentDirectory, fullPath);
                    if (!File.Exists(fullPath))
                        fullPath = null;
                }

                if (fullPath == null)
                    return;

                var pdfBytes = File.ReadAllBytes(fullPath);

                attachments.Add(new JObject
                {
                    {"ContentType", att.MediaType},
                    {"Filename", fileName},
                    {"Base64Content", Convert.ToBase64String(pdfBytes)}
                });



            });



        




        var mObject = new JObject
            {
                {
                    "From",
                    new JObject
                    {
                        {"Email", m.From.Address},
                        {"Name", m.From.DisplayName}
                    }
                },
                {
                    "To", tos
                },
                {
                    "Bcc", bccs
                },
                {
                    "Cc", ccs
                },
                {
                    "Subject",
                    m.Subject
                },
                {
                    "HTMLPart",
                    m.Body
                },
                {"Attachments", attachments}
            };


            msg.Property(Send.Messages, new JArray
            {
                mObject
            });

            ///TODO: no probada aun
            var f = m.ReplyTo.First();
            if (f != null)
            {
                msg.Property(Send.Headers, new JObject {
                    {"Reply-To", f.Address}
                });
            }


            return msg;


        }

    }
}