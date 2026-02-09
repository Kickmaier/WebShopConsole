using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Models
{
    internal class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string StreetAdress { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string? CustomerEmail { get; set; }
        public bool Newsletter { get; set; } = false;
        public string NewsLetterText => Newsletter ? "Ja" : "Nej";
        public int TotalOrderPrice { get; set; }
        public bool IsCanceld { get; set; } = false;
        public virtual Customer? Customer { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }  = new List<OrderItem>();
        public Order() {}
        public int GetVatInOre() => (int)Math.Round(TotalOrderPrice * 0.20);
        public string GetSkrPrice() => (TotalOrderPrice / 100m).ToString("C");
        public string GetVatPrice() => (GetVatInOre() / 100m).ToString("C");
        public string GetPriceExVat() => ((TotalOrderPrice - GetVatInOre()) / 100m).ToString("C");
    }
}
