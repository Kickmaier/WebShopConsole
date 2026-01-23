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
        public string ScientificName { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public Sunlight Sunlight { get; set; }
        public Water Water { get; set; }
        public LifeCycle LifeCycle { get; set; }
        public int InStock { get; set; }

        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

        public Product() {}

        public Product(string name, string scientificName, int price, string description, Sunlight sunlight, 
            Water water, LifeCycle lifeCycle, int inStock, List<Category>categories)
        {
            Name = name;
            ScientificName = scientificName;
            Price = price;
            Description = description;
            Sunlight = sunlight;
            Water = water;
            LifeCycle = lifeCycle;
            InStock = inStock;
            Categories = categories;
        }
        public string GetSkrPrice() => (Price / 100m).ToString("C");

        public string GetSweSun()
        {
            return Sunlight switch
            {
                Sunlight.FullSun => "Full sol",
                Sunlight.MediumShade => "Halvskugga",
                Sunlight.Shade => "Skugga",
                Sunlight.None => ""
            };
        }
        public string GetSweWater()
        {
            return Water switch
            {
                Water.Low => "Mindre ofta",
                Water.Medium => "Medel",
                Water.High => "Ofta",
                Water.None => ""
            };
        }
        public string GetSweLifecycle()
        {
            return LifeCycle switch
            {
                LifeCycle.Annual => "Ettårig Sommarblomma",
                LifeCycle.Biennial => "Tvåårig",
                LifeCycle.Perennial => "Flerårig",
                LifeCycle.None => ""
            };
        }
    }
}
