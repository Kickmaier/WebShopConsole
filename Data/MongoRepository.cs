using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Models;
using WebShop.Data;
using MongoDB.Driver.Linq;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Diagnostics;


namespace WebShop.Data
{
    internal class MongoRepository
    {
        private static readonly IMongoCollection<MongoLogEntry> _logCollection;
        static  MongoRepository()
        {
            _logCollection = MongoDbConfig.GetLogCollection();
        }
        internal static async Task ActivityLoggerAsync(string type, string description, string customer, int? quantity = null)
        {
            var newMongoLogEntry = new MongoLogEntry
            {
                LogType = type,
                Description = description,
                Customer = customer,
                Quantity = quantity
            };

            await _logCollection.InsertOneAsync(newMongoLogEntry);
        }
        internal static async Task<List<(string search, int count)>> TopSearchAsync()
        {
            var topSearc = await _logCollection.AsQueryable()
                .Where(l => l.LogType == "search")
                .GroupBy(l => l.Description)
                .Select(g => new { search = g.Key, count = g.Count() })
                .OrderByDescending (x => x.count)
                .Take(10)
                .ToListAsync();
            
            return topSearc.Select(s => (s.search, s.count)).ToList();
        }
    }
}
