using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using Microsoft.Azure.ServiceBus;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Newtonsoft.Json;
using Functions.Model;

namespace Functions
{
    public static class CarModelSubscriber
    {
        [ServiceBusAccount("ServiceBusConnection")]
        [FunctionName("CarModelSubscriber")]
        public static void Run([ServiceBusTrigger("promcarmodel", "3series", AccessRights.Listen)]BrokeredMessage mySbMsg, TraceWriter log)
        {
            log.Info($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            
            var stream = mySbMsg.GetBody<Stream>();
            StreamReader reader = new StreamReader(stream);
            string obj = reader.ReadToEnd();
            PromoData promoData = JsonConvert.DeserializeObject<PromoData>(obj);

            //save promotion data into mongo collection..........................
        }
    }
}
