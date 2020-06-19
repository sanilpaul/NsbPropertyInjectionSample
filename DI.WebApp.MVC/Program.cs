using System;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NServiceBus;

namespace DI.WebApp.MVC
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.StartAsync();

            Console.WriteLine("Press any key to shutdown");
            Console.ReadKey();
            await host.StopAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .UseNServiceBus(c =>
                {
                    var endpointConfiguration = new EndpointConfiguration("DI.WebApp.Mvc");
                    endpointConfiguration.UseTransport<LearningTransport>();
                    return endpointConfiguration;
                });
    }
}
