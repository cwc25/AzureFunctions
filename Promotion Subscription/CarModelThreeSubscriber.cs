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
using Functions.Connections;
using StackExchange.Redis;

namespace Functions
{
    public static class CarModelThreeSubscriber
    {
        [ServiceBusAccount("ServiceBusConnection")]
        [FunctionName("CarModelThreeSubscriber")]
        public static void Run([ServiceBusTrigger("promcarmodel", "3series", AccessRights.Listen)]BrokeredMessage mySbMsg, TraceWriter log)
        {
            log.Info($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            
            var stream = mySbMsg.GetBody<Stream>();
            StreamReader reader = new StreamReader(stream);
            string obj = reader.ReadToEnd();
            PromoData promoData = JsonConvert.DeserializeObject<PromoData>(obj);

            //save promotion posts into mongo collection..........................
            MongoDBConnection.GetCollection<PromoData>("PromotionCollectionName").InsertOne(promoData);

            //save promotion posts into redis list and trim for the latest 5 posts.
            IDatabase cache = RedisConnection.Connection.GetDatabase();
            string redisKey = System.Environment.GetEnvironmentVariable("key:3seriespromotionpost");
            cache.ListLeftPush(redisKey, obj);
            cache.ListTrim(redisKey, 0, 5);
        }
    }
}
