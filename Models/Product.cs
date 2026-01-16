using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Models
{
    internal class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
