using System.Linq;
using System.Net;
using System.Net.Http;
using System.Configuration;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Security.Authentication;
using Functions.Model;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Functions.GetAllFunction
{
    public static class GetAllFunction
    {
        [FunctionName("GetAllFunction")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getall")]HttpRequestMessage req, TraceWriter log)
        {
            
            var driverData = DriverDataCollection.GetCollection().Find(new BsonDocument()).ToList();
            // Fetching the name from the path parameter in the request URL test
            return req.CreateResponse(HttpStatusCode.OK, "2");
        }
    }
}