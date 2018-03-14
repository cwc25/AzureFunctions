using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Functions.Connections
{
    public static class RedisConnection
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            ConfigurationOptions config = new ConfigurationOptions();
            string endpoint = System.Environment.GetEnvironmentVariable("RedisEndpoint");
            string password = System.Environment.GetEnvironmentVariable("RedisPwd");
            config.EndPoints.Add(endpoint);
            config.Password = password;
            config.Ssl = true;
            config.AbortOnConnectFail = false;
            config.ConnectRetry = 5;
            config.ConnectTimeout = 5000;
            return ConnectionMultiplexer.Connect(config);
        });

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
    }
}
