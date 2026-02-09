using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebShop.Models;

namespace WebShop.Data
{
    internal class MongoDbConfig
    {
        private static MongoClient GetClient()
        {
            var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();
            var connectionString = config["MySettings:MongoDbConnectionString"];
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            var client = new MongoClient(settings);
            return client;
        }

        internal static IMongoCollection<MongoLogEntry> GetLogCollection()
        {
            var client = GetClient();

            var dataBase = client.GetDatabase("WebShopLogs");

            return dataBase.GetCollection<MongoLogEntry>("ActivityLogs");
        }
    }
}
