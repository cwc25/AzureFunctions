using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Functions.GetDataById
{
    public static class GetDataById
    {
        [FunctionName("GetDataById")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "id/{name}")]HttpRequestMessage req, string name, TraceWriter log)
        {
            string query = "{VehicleId:{$eq:\"" + name + "\"}}";
            var data = MongoDBConnection.GetCollection<dynamic>("DriverCollectionName").Find(query).ToList();

            // Fetching the name from the path parameter in the request URL test
            return req.CreateResponse(HttpStatusCode.OK, data, JsonMediaTypeFormatter.DefaultMediaType);
        }
    }
}
