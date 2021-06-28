using System;
using System.IO;
using System.Threading.Tasks;
using Desyco.Notification;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebNotifier.Service;


namespace WebNotifier
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddLogging();
            
            services.AddNotification(opt =>
            {

                //Background Worker forwarding service configuration
                opt.DisabledForwardingServiceWorker = false;
                opt.DeliveryAttemptsDelay = 5;
                opt.MaxDeliveryAttempts = 5;

                //opt.UseDefaultMailProvider(cfg =>
                //{
                //    cfg.FromAddress = "sample@gmail.com";
                //    cfg.SenderName = "Notification Service";
                //    cfg.UseDefaultCredentials = false;
                //    cfg.SmtpServerProfiles.Add(new SmtpParameters
                //    {
                //        EnableSsl = true,
                //        UserName = "sample@gmail.com",
                //        Password = "*********",
                //        SmtpServer = "smtp.gmail.com",
                //        Port = 587
                //    });
                //});

                opt.UseMailKitProvider(cfg =>
                {

                    cfg.FromAddress = "sample@gmail.com";
                    cfg.SenderName = "Notification Service";

                    cfg.SmtpServerProfiles.Add(new SmtpParameters
                    {
                        EnableSsl = true,
                        UserName = "sample@gmail.com",
                        Password = "******",
                        SmtpServer = "smtp.gmail.com",
                        Port = 465
                    });

                });


                opt.UseT5FileTemplating(Path.Combine(Directory.GetCurrentDirectory(), "templates"));

                //opt.AddSignalR(hubOptions => { hubOptions.EnableDetailedErrors = true; });


                opt.Events = new NotificationEvents
                {
                    OnNotificationStart = arg =>
                    {
                        Console.WriteLine($"Message id {arg.Message.Group} is ready to be sent");
                        return Task.CompletedTask;
                    },
                    OnNotificationSending = arg =>
                    {
                        Console.WriteLine($"Message id {arg.Message.Group} was Sent.");
                        return Task.CompletedTask;
                    },
                    OnNotificationDelivered = arg =>
                    {
                        Console.WriteLine($"Message id {arg.Message.Group} was Delivered.");
                        return Task.CompletedTask;
                    },
                    OnNotificationFailed = arg =>
                    {
                        Console.WriteLine($"Message id {arg.Message.Group} Failed.");
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddSingleton<IRequestFormService, RequestFormService>();

            services.AddSwaggerGen();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseNotification();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint(@"/swagger/v1/swagger.json", @"Notification Service V1");
                    c.RoutePrefix = "";
                }
            );
            app.UseRouting();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
