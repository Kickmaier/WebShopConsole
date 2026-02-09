using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WebShop.Helpers;
using WebShop.Models;
using WebShop.Services;

namespace WebShop.Data
{
    internal class EFRepository
    {
        internal static async Task<Category?> GetCategoryWithProductsAsync(MyDbContext db, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }
            return await db.Categories
                .Include(c => c.Products.Where(p => p.IsOnPage))
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());
        }
        internal static async Task<Product?> GetSingleProductAsync(MyDbContext db, int productId)
        {
            if (productId <= 0)
            {
                return null;
            }
            return await db.Products
                .Include(p => p.Categories)
                .FirstOrDefaultAsync(p => p.Id == productId && p.IsOnPage);
        }
        internal static async Task<List<Product>> GetOnDisplayAsync(MyDbContext db)
        {
            return await db.Products
                .Include(p => p.Categories)
                .Where(p => p.IsOnDisplay)
                .Take(3)
                .ToListAsync();
        }
        internal static async Task AddCartItemAsync(MyDbContext db, int productId)
        {
            int? currentUserId = CurrentSession.isLoggedIn ? CurrentSession.CurrentCustomer.Id : null;

            var product = await db.Products
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                Console.WriteLine("Produkten finns inte.");
                return;
            }
            if (product.InStock <= 0)
            {
                Console.WriteLine("Produkten är slut i lager.");
                return;
            }
            var currentItem = await db.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == productId && ci.CustomerId == currentUserId);

            if (currentItem != null)
            {
                if (currentItem.Quantity + 1 > product.InStock)
                {
                    Console.WriteLine("Det finns inte tillräckligt många i lager.");
                    return;
                }
                    currentItem.Quantity++;
            }
            else
            {
                await db.CartItems.AddAsync(new CartItem { ProductId = productId, Quantity = 1, CustomerId = currentUserId });
            }
            await db.SaveChangesAsync();
        }
        internal static async Task DeleteFromCartAsync(MyDbContext db, int productId)
        {
            int? currentUserId = CurrentSession.isLoggedIn ? CurrentSession.CurrentCustomer.Id : null;
            var currentItem = await db.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == productId && ci.CustomerId == currentUserId);

            if (currentItem != null)
            {
                db.CartItems.Remove(currentItem);
                await db.SaveChangesAsync();
            }
        }
        internal static async Task RemoveCartItemAsync(MyDbContext db, int productId)
        {
            int? currentUserId = CurrentSession.isLoggedIn ? CurrentSession.CurrentCustomer.Id : null;
            var currentItem = await db.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == productId && ci.CustomerId == currentUserId);

            if (currentItem != null)
            {
                currentItem.Quantity--;

                if (currentItem.Quantity <= 0)
                {
                    db.Remove(currentItem);
                }
            }
            await db.SaveChangesAsync();
        }
        internal static async Task<List<CartItem>> GetCartAsync(MyDbContext db)
        {
            int? currentId = CurrentSession.isLoggedIn ? CurrentSession.CurrentCustomer.Id : null;
            return await db.CartItems
            .Include(ci => ci.Product)
            .Where(ci => ci.CustomerId == currentId && ci.Product.IsOnPage)
            .ToListAsync();
        }
        internal static async Task LoginAsync(MyDbContext db)
        {
            Customer customer = null;
            Console.Clear();
            Console.WriteLine("---LOGGA IN---");

            if (CurrentSession.CurrentCustomer != null)
            {
                UserInterface.FullCustomerInfo(CurrentSession.CurrentCustomer);
                return;
            }
            else
            {
                string email = TextHelpers.RequiredField("ange email: ");
                string password = TextHelpers.RequiredField("Ange lösenord: ");
                customer = await db.Customers
                    .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderItems)
                    .FirstOrDefaultAsync(c => c.CustomerEmail == email && c.CustomerPassword == password);
            }
            if (customer != null)
            {
                CurrentSession.CurrentCustomer = customer;
                if (customer.Role == Role.Admin)
                {
                    await AdminUI.Start(db);
                }
                else
                {
                    var guestCart = await db.CartItems
                        .Where(c => c.CustomerId == null)
                        .ToListAsync();
                    foreach (var item in guestCart)
                    {
                        item.CustomerId = customer.Id;
                    }
                    var guestOrders = await db.Orders
                    .Where(o => o.CustomerId == null && o.CustomerEmail.ToLower() == customer.CustomerEmail.ToLower())
                    .ToListAsync();

                    foreach (var order in guestOrders)
                    {
                        order.CustomerId = customer.Id;
                    }
                    await db.SaveChangesAsync();

                    Console.WriteLine("Välkommen " + customer.CustomerName + " du är nu inloggad och kan fortsätta");
                }
            }
            else
            {
                Console.WriteLine("Felaktiga inloggningsuppgifter eller obefintlig kund. \nFör att skapa kund slutför ett köp");
            }
            Thread.Sleep(2000);
        }
        internal static void AddCategory(MyDbContext db)
        {
            Console.WriteLine("Ange namn på kategorin du vill lägga till");
            string name = Console.ReadLine();
            db.Categories.Add(new Category
            {
                Name = name
            });
            db.SaveChanges();
            Console.WriteLine("Kategorin " + name + " tillagd");
            Thread.Sleep(1500);
        }
        internal static void RemoveCategory(MyDbContext db)
        {
            var categoryList = db.Categories.ToList();
            foreach (var c in categoryList)
                Console.WriteLine(c.Name);
            Console.WriteLine();
            Console.WriteLine("Ange namn på kategorin du vill ta bort");
            string name = Console.ReadLine();
            var category = db.Categories
                .Include(c => c.Products)
                .FirstOrDefault(c => c.Name.ToLower() == name.ToLower());
            if (category != null)
            {
                if (category.Products.Count > 0)
                {
                    Console.WriteLine("Kategorin kan inte tas bort, den innehåller produkterna");
                    Console.WriteLine();
                    foreach (Product product in category.Products)
                    {
                        Console.WriteLine(product.Name);
                    }
                }
                else
                {
                    db.Categories.Remove(category);
                    db.SaveChanges();
                    Console.WriteLine("Kategorin " + category.Name + " borttagen");
                }
            }
            else
            {
                Console.WriteLine("Kategorin hittades inte");
            }
            Console.ReadLine();
        }
        internal static void AddProduct(MyDbContext db)
        {
            Console.Clear();
            var newProduct = new Product();
            Console.WriteLine("---LÄGG TILL PRODUKT---");
            Console.WriteLine();
            Console.WriteLine("Informationen kommer kunna ändras i efterhand");
            newProduct.Name = TextHelpers.RequiredField("Ange produktnamn");
            newProduct.ScientificName = TextHelpers.RequiredField("Ange vetenskapligt namn");
            newProduct.Price = TextHelpers.GetIntInput("Ange pris i öre ");
            newProduct.Description = TextHelpers.RequiredField("Ange produktbeskrivning");
            newProduct.Sunlight = AdminUI.SetSunLight();
            newProduct.Water = AdminUI.SetWater();
            newProduct.LifeCycle = AdminUI.SetLifeCycle();
            newProduct.InStock = TextHelpers.GetIntInput("Ange lagersaldo ");
            Console.Clear();
            var categories = db.Categories.ToList();
            Console.WriteLine("Id:,-4" + " | Namn:");
            Console.WriteLine(new string('-', 40));
            foreach (var c in categories)
            {
                Console.WriteLine($"{c.Id}: {c.Name}");
            }
            Console.WriteLine("Välj 3 kategorier att knyta till produkten via Id");

            for (int i = 0; i < 3; i++)
            {
                int choice;
                string input = Console.ReadLine();
                while (!int.TryParse(input, out choice))
                {
                    Console.WriteLine("Ange en befintlig kategori");
                }
                var category = db.Categories.Find(choice);
                if (category != null)
                {
                    newProduct.Categories.Add(category);
                    Console.WriteLine(category.Name + " tillagd");
                }
                else
                {
                    Console.WriteLine("Kategorin hittades inte försök med ett annat \"Id\"");
                }
            }
            db.Products.Add(newProduct);
            db.SaveChanges();
            Console.WriteLine("Produkt " + newProduct.Name + " tillagd");
            Thread.Sleep(1000);
        }
        internal static void ProductVisibility(MyDbContext db)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--- HANTERA SYNLIGHET ---");
                Console.WriteLine("[1] Inaktivera produkt (Gör dolda)");
                Console.WriteLine("[2] Aktivera produkt (Visa i butik)");
                Console.WriteLine("[0] Gå tillbaka");

                ConsoleKey key = Console.ReadKey(true).Key;
                var (action, title) = key switch
                {
                    ConsoleKey.D1 or ConsoleKey.NumPad1 => (1, "---VISA PRODUKT---"),
                    ConsoleKey.D2 or ConsoleKey.NumPad2 => (2, "---DÖLJ PRODUKT---"),
                    _ => (0, "")
                };
                if (action == 0) return;
                List<Product> products;
                if (action == 1)
                {
                    products = db.Products.Where(p => p.IsOnPage == true).ToList();
                }
                else
                {
                    products = db.Products.Where(p => p.IsOnPage == false).ToList();
                }
                Console.Clear();
                Console.WriteLine(title);
                Console.WriteLine(new string('-', 40));
                if (!products.Any())
                {
                    Console.WriteLine("Inga produkter matchar valet!");
                    Console.WriteLine("Tryck på valfi tangent för att fortsätta");
                    Console.ReadKey(true);
                    continue;

                }
                foreach (var p in products)
                {
                    Console.WriteLine($"{p.Id,-4} | {p.Name}");
                }
                int input = TextHelpers.GetIntInput("Ange produktid du vill ändra displayläge på, eller 0 för att återgå!");

                if (input == 0) continue;
                var selected = products.FirstOrDefault(p => p.Id == input);
                if (selected != null)
                {
                    selected.IsOnPage = !selected.IsOnPage;
                    db.SaveChanges();
                    Console.WriteLine("Visibilitet ändrad!");
                }
                else
                {
                    Console.WriteLine("Kunde inte hitta produkten på listan!");
                }
                Console.WriteLine("Tryck på valfri tangent!");
                Console.ReadKey(true);
            }
        }
        internal static void EditProduct(MyDbContext db)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("---REDIGERA PRODUKT---");
                var allProducts = db.Products.ToList();
                Console.WriteLine($"{"Id",-4} | Namn; ");
                foreach (var p in allProducts)
                {
                    Console.WriteLine($"{p.Id,-4} | {p.Name}");
                }
                int input = TextHelpers.GetIntInput("Ange produkt Id på produkten du vill redigera eller 0 för att gå tillbaka");
                var product = db.Products
                                .Include(p => p.Categories)
                                .FirstOrDefault(p => p.Id == input);
                if (input == 0)
                {
                    return;
                }
                if (product != null)
                {
                    bool edit = true;
                    while (edit)
                    {
                        Console.Clear();
                        UserInterface.DisplaySingleProduct(product);

                        Console.WriteLine("---VAD VILL DU ÄNDRA?---");
                        Console.WriteLine("[1]Namn:");
                        Console.WriteLine("[2]Vetenskapligt Namn:");
                        Console.WriteLine("[3]Pris:");
                        Console.WriteLine("[4]Beskrivning:");
                        Console.WriteLine("[5]Solljus:");
                        Console.WriteLine("[6]Vatten:");
                        Console.WriteLine("[7]Livscykel:");
                        Console.WriteLine("[8]Lagersaldo:");
                        Console.WriteLine("[9]Grossist:");
                        Console.WriteLine("[K]Kategorier:");
                        Console.WriteLine("[S]Spara:");
                        Console.WriteLine("[Q]Avsluta");

                        ConsoleKeyInfo key = Console.ReadKey(true);

                        switch (key.Key)
                        {
                            case ConsoleKey.D1:
                            case ConsoleKey.NumPad1:
                                product.Name = TextHelpers.RequiredField("Ange nytt namn");
                                break;
                            case ConsoleKey.D2:
                            case ConsoleKey.NumPad2:
                                product.ScientificName = TextHelpers.RequiredField("Ange vetenskapligt namn");
                                break;
                            case ConsoleKey.D3:
                            case ConsoleKey.NumPad3:
                                product.Price = TextHelpers.GetIntInput("Ange pris");
                                break;
                            case ConsoleKey.D4:
                            case ConsoleKey.NumPad4:
                                product.Description = TextHelpers.RequiredField("Ange ny beskrivning");
                                break;
                            case ConsoleKey.D5:
                            case ConsoleKey.NumPad5:
                                product.Sunlight = AdminUI.SetSunLight();
                                break;
                            case ConsoleKey.D6:
                            case ConsoleKey.NumPad6:
                                product.Water = AdminUI.SetWater();
                                break;
                            case ConsoleKey.D7:
                            case ConsoleKey.NumPad7:
                                product.LifeCycle = AdminUI.SetLifeCycle();
                                break;
                            case ConsoleKey.D8:
                            case ConsoleKey.NumPad8:
                                product.InStock = TextHelpers.GetIntInput("Ange lagersaldo");
                                break;
                            case ConsoleKey.D9:
                            case ConsoleKey.NumPad9:
                                AdminUI.ChangeWholesale(db, product);
                                break;
                            case ConsoleKey.K:
                                AdminUI.EditProductCategories(db, product);
                                break;
                            case ConsoleKey.S:
                                db.SaveChanges();
                                Console.WriteLine("Ändringar Sparade");
                                Thread.Sleep(1000);
                                edit = false;
                                break;
                            case ConsoleKey.Q:
                                if (TextHelpers.YesNoReturn("Vill du spara innan du går vidare?"))
                                {
                                    db.SaveChanges();
                                    Console.WriteLine("Sparat!");
                                    Thread.Sleep(1000);
                                }
                                edit = false;
                                break;
                        }
                    }

                }
                else
                {
                    Console.WriteLine("Kunde inte hitta produkten");
                    Thread.Sleep(1000);
                }
            }
        }
        internal static Customer GetCustomerWithHistory(MyDbContext db, string email)
        {
            return db.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderItems)
                .FirstOrDefault(c => c.CustomerEmail.ToLower() == email.ToLower());
        }
        internal static async Task<List<Product>> SeachProductAsync(MyDbContext db, string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return new List<Product>();
            }
            string searchToLower = search.ToLower();

            return await db.Products
                    .Include(p => p.Categories)
                    .Where(p => p.Name.ToLower().Contains(searchToLower) ||
                    p.Categories.Any(c => c.Name.ToLower().Contains(searchToLower)))
                    .ToListAsync();
        }

        //Lite kod som hjälpt i arbetet med uppgiften
        //Ge addminrättigheter till customer med ett specifikt id
        //internal static void CreateAdmin(MyDbContext db, int customerId)
        //{
        //    var newAdmin = db.Customers
        //        .FirstOrDefault(c => c.Id == customerId);

        //    if (newAdmin != null)
        //    {
        //        newAdmin.Role = Role.Admin;
        //        Console.WriteLine(newAdmin.CustomerName + " har nu adminrättigheter");
        //        db.SaveChanges();
        //    }
        //    else
        //    {
        //        Console.WriteLine(customerId + " kunde inte hittas!");

        //        return;
        //    }
        //    Thread.Sleep(1000);
        //}

        //Rensa ordrar
        //internal static void DeleteAllOrders(MyDbContext db)
        //{
        //    db.Orders.RemoveRange(db.Orders.ToList());
        //    db.SaveChanges();
        //}



        //För att rensa en hel tabell i detta fall CartItems
        //public static void RemoveItem(MyDbContext db)
        //{
        //    db.CartItems.RemoveRange(db.CartItems);
        //    db.SaveChanges();
        //}
    }
}


