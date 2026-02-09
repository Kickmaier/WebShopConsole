using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Models
{
    internal class Customer
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastOrder { get; set; } = DateTime.Now;
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAdress { get; set; }
        public string ZipCode { get; set; }
        public string CustomerCity { get; set; }
        public string CustomerPassword { get; set; }
        public bool CustomerNewsLetter { get; set; }
        public string NewsLetterText => CustomerNewsLetter ? "Ja" : "Nej";
        public Role Role { get; set; } = Role.Customer;

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public Customer() {}
        
        public Customer (string customerName, string customerEmail, string customerAdress, string zipCode, string customerCity, string customerPassword, bool customerNewsLetter)
        {
            {
                CustomerName = customerName;
                CustomerEmail = customerEmail;
                CustomerAdress = customerAdress;
                ZipCode = zipCode;
                CustomerCity = customerCity;
                CustomerPassword = customerPassword;
                CustomerNewsLetter = customerNewsLetter;
            } 
        }
    }
}
