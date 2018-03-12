using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Functions.Model;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace Functions.Promotion_Subscription
{
    public static class PublishPromotion
    {
        [FunctionName("PublishPromotion")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "publishpromotion")]HttpRequestMessage req, TraceWriter log)
        {
            string connection = System.Environment.GetEnvironmentVariable("ServiceBusConnection");
            string topic = System.Environment.GetEnvironmentVariable("promotiontopic");
            string data = await req.Content.ReadAsStringAsync();
            var promoData = JsonConvert.DeserializeObject<PromoData>(data);
            Message message = new Message(Encoding.UTF8.GetBytes(data))
            {
                UserProperties =
                {
                    { "series", promoData.Model },
                    { "priority", promoData.Priority }
                }
            };
            ITopicClient topicClient = new TopicClient(connection, topic);
            await topicClient.SendAsync(message);

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
