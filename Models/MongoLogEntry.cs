using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebShop.Models
{
    public class MongoLogEntry
    {
        public ObjectId Id { get; set; }
        public string LogType { get; set; } = "";
        public string Description { get; set; } = "";
        public string Customer { get; set; } = "Gäst";
        public int? Quantity { get; set; } = null;

        public DateTime LoggedTime { get; set; } = DateTime.Now;
    }
}
