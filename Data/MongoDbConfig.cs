using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Data
{
    internal class MongoDbConfig
    {
        //private static MongoClient GetClient()
        //{
        //    string connectionString = "mongodb+srv://Kickmaier:System25%21@demo.h5gfh51.mongodb.net/?appName=Demo";
        //    MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
        //    var client = new MongoClient(settings);
        //    return client;
        //}

        //public static IMongoCollection<Models.Person> GetPersonCollection()
        //{
        //    var client = GetClient();

        //    var dataBase = client.GetDatabase("MyPersonDb");
        //    var personCollection = dataBase.GetCollection<Models.Person>("PersonCollection");
        //    return personCollection;
        //}
    }
}
