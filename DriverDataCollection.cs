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

namespace Functions
{
    public static class DriverDataCollection
    {
        public static IMongoCollection<dynamic> GetCollection(string collectionName)
        {
            var host = System.Environment.GetEnvironmentVariable("MongoHost");
            var collection = System.Environment.GetEnvironmentVariable(collectionName);
            var userName = System.Environment.GetEnvironmentVariable("MongoUserName");
            var password = System.Environment.GetEnvironmentVariable("MongoPwd");
            var dbName = System.Environment.GetEnvironmentVariable("MongoDBName");

            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress(host, 10255);
            settings.UseSsl = true;
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;
            //hotfix fix2 test
            MongoIdentity identity = new MongoInternalIdentity(dbName, userName);
            MongoIdentityEvidence evidence = new PasswordEvidence(password);
            List<MongoCredential> credentials = new List<MongoCredential>();
            credentials.Add(new MongoCredential("SCRAM-SHA-1", identity, evidence));
            settings.Credentials = credentials;
            MongoClient client = new MongoClient(settings);
            var database = client.GetDatabase(dbName);

            return database.GetCollection<dynamic>(collection);
        }
    }
}
