using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace DI.EndpointA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .UseConsoleLifetime()
                .ConfigureLogging((ctx, logging) =>
                {
                    logging.AddConfiguration(ctx.Configuration.GetSection("Logging"));
                    logging.AddEventLog();
                    logging.AddConsole();
                })
                .UseNServiceBus(ctx =>
                {
                    var endpointConfiguration = new EndpointConfiguration("DotnetCore.DI");
                    endpointConfiguration.SendFailedMessagesTo("DotnetCore.DI.Error");
                    endpointConfiguration.UseTransport<LearningTransport>();
                    
                    var recoverability = endpointConfiguration.Recoverability();
                    recoverability.Immediate(immediate => { immediate.NumberOfRetries(0); });
                    recoverability.Delayed(delayedRetriesSettings => { delayedRetriesSettings.NumberOfRetries(0); });

                    endpointConfiguration.DefineCriticalErrorAction(OnCriticalError);
                    
                    return endpointConfiguration;
                })
                .ConfigureServices(ConfigureServices);

            return builder;
        }

        public static void ConfigureServices(HostBuilderContext hostingContext, IServiceCollection services)
        {
            services.AddHostedService<RunOnStartup>();
        }
       
        private static async Task OnCriticalError(ICriticalErrorContext context)
        {
            var fatalMessage = $"The following critical error was encountered:{Environment.NewLine}{context.Error}{Environment.NewLine}Process is shutting down. StackTrace: {Environment.NewLine}{context.Exception.StackTrace}";
            EventLog.WriteEntry(".NET Runtime", fatalMessage, EventLogEntryType.Error);
            try
            {
                await context.Stop().ConfigureAwait(false);
            }
            finally
            {
                Environment.FailFast(fatalMessage, context.Exception);
            }
        }
    }
}