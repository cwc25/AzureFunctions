using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;

namespace Functions
{
    public static class CarModelSubscriber
    {
        [ServiceBusAccount("ServiceBusConnection")]
        [FunctionName("CarModelSubscriber")]
        public static void Run([ServiceBusTrigger("promcarmodel", "3series", AccessRights.Listen)]string mySbMsg, TraceWriter log)
        {
            log.Info($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
    }
}
