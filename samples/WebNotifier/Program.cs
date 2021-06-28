using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WebNotifier
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    //.ConfigureAppConfiguration((hostingContext, config) =>
                    //{
                    //    config.Add(new NotificationConfigurationSource()
                    //    {
                    //        Options = o => o.UseSqlServer(@"Server=.;Database=NotificationDb;Trusted_Connection=True;")
                    //    });
                    //});

                });
    }
}
