using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Models
{
    internal class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
        public int PriceAtPurchase { get; set; }
        public Order Order { get; set; }
        public OrderItem() { }
        public OrderItem(int productId, string productName, int productQuantity, int priceAtPurchase)
        {
            ProductId = productId;
            ProductName = productName;
            ProductQuantity = productQuantity;
            PriceAtPurchase = priceAtPurchase;
        }
        public string GetSkrPrice() => (PriceAtPurchase / 100m).ToString("C");

    }
}
