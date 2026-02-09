using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop.Data
{
    internal class MyDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();
            var connStr = config["MySettings:ConnectionString"];
            optionsBuilder.UseSqlServer(connStr);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //SÄTTER ONDELETET TILL RESTRICTED FÖR CATEGORY I SKUGGTABELLEN PRODUCTCATEGORY
            modelBuilder.Entity<Product>(entity => 
                {
                entity.HasMany(p => p.Categories)
                .WithMany(c => c.Products)
                .UsingEntity<Dictionary<string, object>>(
                "ProductCategory",
                j => j.HasOne<Category>().WithMany().OnDelete(DeleteBehavior.Restrict),
                j => j.HasOne<Product>().WithMany().OnDelete(DeleteBehavior.Cascade));
                
                //SÄTTER DATATYPERNA PÅ ENUMS TILL STRING NÄR DE SPARAS FÖR LÄSBARHET        

                entity.Property(p => p.Sunlight)
               .HasConversion<string>()
               .HasColumnType("nvarchar(20)");

                entity.Property(p => p.Water)
               .HasConversion<string>()
               .HasColumnType("nvarchar(20)");
               
                entity.Property(p => p.LifeCycle)
                .HasConversion<string>()
                .HasColumnType("nvarchar(20)");  
                
                });           
        }
        public DbSet<Product> Products {get; set;}
        public DbSet<Category> Categories {get; set;}
        public DbSet<CartItem> CartItems {get; set;}
        public DbSet<Order> Orders {get; set;}
        public DbSet<OrderItem> OrderItems {get; set;}
        public DbSet<Customer> Customers {get; set;}
        public DbSet<Wholesaler> Wholesalers {get; set;}
    }
}
