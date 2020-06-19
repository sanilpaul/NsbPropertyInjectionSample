using System.Threading.Tasks;
using DI.ServiceA;
using Microsoft.Extensions.Logging;
using NServiceBus;

namespace DI.EndpointA
{
    public class TestConstructorInjectionHandler : IHandleMessages<TestConstructorInjection>
    {
        private readonly IDoSomeServiceAWork someServiceAWork;
        private readonly ILogger<TestConstructorInjectionHandler> logger;
        public TestConstructorInjectionHandler(IDoSomeServiceAWork someServiceAWork, ILogger<TestConstructorInjectionHandler> logger)
        {
            this.someServiceAWork = someServiceAWork;
            this.logger = logger;
        }

        public Task Handle(TestConstructorInjection message, IMessageHandlerContext context)
        {
            logger.LogInformation("***** Constructor Injection *********");
            logger.LogInformation(someServiceAWork.DoSomething());

            return Task.CompletedTask;
        }
    }
}