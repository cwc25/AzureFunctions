using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using Functions.Connections;
using Functions.Model;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace Functions.Promotion_Subscription
{
    public static class GetPromoData
    {
        [FunctionName("GetPromoData")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, 
                                              "get", Route = "promodata/{series}")]HttpRequestMessage req, 
                                              string series, 
                                              TraceWriter log)
        {
            IDatabase cache = RedisConnection.Connection.GetDatabase();
            string redisKey = System.Environment.GetEnvironmentVariable("RedisKeyModelThree");
            List<PromoData> promos = new List<PromoData>();
            var length = cache.ListLength(redisKey);

            if (length > 0)//in redis
            {
                foreach (string item in cache.ListRange(redisKey, 0, (length - 1)))
                {
                    promos.Add(JsonConvert.DeserializeObject<PromoData>(item));
                }
            }
            else//get from mongodb and insert into redis
            {
                var promoCollection = MongoDBConnection.GetCollection<PromoData>("PromotionCollectionName");
                promos = promoCollection.AsQueryable().OrderByDescending(p => p.CreatedTime).Take(5).ToList();

                log.Info(promos.Count().ToString());

                foreach(var promo in promos)
                {
                    cache.ListLeftPush(redisKey, JsonConvert.SerializeObject(promo));
                }

                cache.ListTrim(redisKey, 0, 4);
            }

            return req.CreateResponse(HttpStatusCode.OK, promos, JsonMediaTypeFormatter.DefaultMediaType);
        }
    }
}
