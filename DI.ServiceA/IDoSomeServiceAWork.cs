using Microsoft.Extensions.Logging;
using NServiceBus;

namespace DI.ServiceA
{
    public interface IDoSomeServiceAWork
    {
        string DoSomething();
    }

    public class SomeServiceAWork : IDoSomeServiceAWork
    {
        private readonly ILogger<SomeServiceAWork> logger;

        public SomeServiceAWork(ILogger<SomeServiceAWork> logger)
        {
            this.logger = logger;
        }

        public string DoSomething()
        {
            logger.LogInformation("Doing SERVICE A work");

            return "Hello From Service A";
        }
    }

    public class Configure : INeedInitialization
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            endpointConfiguration.RegisterComponents(c => c.ConfigureComponent<SomeServiceAWork>(DependencyLifecycle.InstancePerCall));
        }
    }
}
