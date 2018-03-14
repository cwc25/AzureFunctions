using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Functions.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace Functions.Circle
{
    public static class AddCircleContent
    {
        [FunctionName("AddCircleContent")]
        public async static Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "content/{id}")]HttpRequestMessage req, string id, TraceWriter log)
        {
            //add content into MongoDB
            dynamic data = await req.Content.ReadAsStringAsync();
            var circleData = JsonConvert.DeserializeObject<CircleData>(data as string);
            MongoDBConnection.GetCollection<CircleData>("CircleCollectionName").InsertOne(circleData);

            return req.CreateResponse(HttpStatusCode.OK, "Inserted");
        }
    }
}
