using System.Threading.Tasks;
using DI.ServiceA;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace DI.EndpointA
{
    public class TestPropertyInjectionHandler : IHandleMessages<TestPropertyInjection>
    {
        public ILogger<TestPropertyInjectionHandler> Logger { get; set; }

        public IDoSomeServiceAWork ServiceWorker { get; set; }
        public Task Handle(TestPropertyInjection message, IMessageHandlerContext context)
        {
            Logger.LogInformation("***** Property Injection *********");
            Logger.LogInformation(ServiceWorker.DoSomething());
            
            return Task.CompletedTask;
        }
    }

    public class TestPropertyInjection : IMessage
    {
    }
}