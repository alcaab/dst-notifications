using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Desyco.Notification;
using Microsoft.Extensions.DependencyInjection;

namespace SendMailSample
{
    public partial class Form1 : Form
    {
        private IServiceProvider _serviceProvider;
        private INotificationHost _host;

        public Form1()
        {
            InitializeComponent();


        }


        private static IServiceProvider ConfigureServices()
        {
            //setup dependency injection
            var services = new ServiceCollection();

            services.AddNotification(opt =>
            {
                opt.UseMailKitProvider(cfg =>
                {

                    cfg.ServerProfiles.Add(new SmtpParameters
                    {
                        EnableSsl = true,
                        UserName = "contactomopc@gmail.com",
                        Password = "Mopcti2021",
                        FromAddress = "contactomopc@gmail.com",
                        SenderName = "Notification Service",
                        ServerHostName = "smtp.gmail.com",
                        Port = 465
                    });

                });

            });

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _serviceProvider = ConfigureServices();
            var _host = _serviceProvider.GetService<INotificationHost>();

            _host.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var m = new NotificationMessage
            {
                NotificationMethod = NotificationMethod.External,
                Subject = "Prueba de envio de Notificacion",
                Content = "Esta es una prueba de contenido del mesaje",
                To = new List<NotificationAddress>()
                {
                    new NotificationAddress("alcaab@gmail.com", "alcaab2", "Alexis Castro")
                },
            };

            _host.Notify(m);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _host.Stop();
        }
    }
}
