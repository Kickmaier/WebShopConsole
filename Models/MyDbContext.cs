using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Models
{
    internal class MyDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=WebShopInlämning;Trusted_Connection=True; TrustServerCertificate=True;");
        }
        public DbSet<Product> Products {get; set;}
        public DbSet<Category> Categories {get; set;}
    }
}
