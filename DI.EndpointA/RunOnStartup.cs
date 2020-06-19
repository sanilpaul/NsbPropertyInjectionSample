using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace DI.EndpointA
{
    class RunOnStartup : BackgroundService
    {
        private readonly IServiceProvider provider;
        public RunOnStartup(IServiceProvider provider)
        {
            this.provider = provider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var session = provider.GetService<IMessageSession>();
            var logger = provider.GetService<ILogger<RunOnStartup>>();
            
            logger.LogInformation("Starting Run On Startup");
            
            await session.SendLocal(new TestConstructorInjection());
            await session.SendLocal(new TestPropertyInjection());
        }
    }

    public class TestConstructorInjection : IMessage
    {
    }
}