using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Models;
using WebShop.Data;
using WebShop.Helpers;
using System.Xml.Serialization;
using MongoDB.Driver;
using System.Reflection.Metadata.Ecma335;
using System.Diagnostics;


namespace WebShop.Services
{
    internal class AdminUI
    {
        internal static async Task Start(MyDbContext db)
        {
            await AdminMainMenu(db);
        }
        internal static async Task AdminMainMenu(MyDbContext db)
        {
            if (!Security.AuthorizeAdmin()) return;

            while (true)
            {
                UserInterface.DisplayLogo();
                Console.Clear();
                Console.WriteLine("---ADMIN MENY---");
                Console.WriteLine();
                foreach (AdminMenu choice in Enum.GetValues(typeof(AdminMenu)))
                {
                    Console.WriteLine("[" + (char)choice + "] " + choice);
                }

                var key = Console.ReadKey(true);
                AdminMenu selection = (AdminMenu)key.KeyChar;

                Action action = selection switch
                {
                    AdminMenu.Kategorihantering => () => CategoryManager(db),
                    AdminMenu.Produkthantering => () => ProductManager(db),
                    AdminMenu.Kundhantering => () => EditCustomer(db),
                    AdminMenu.OnDisplayIsOnPage => () => OnDisplayAdmin(db),
                    AdminMenu.Statistik => async () => await Statistics(db),
                    AdminMenu.Lagerstatus => () => StockSaldo(db),
                    AdminMenu.Utloggning => () => CurrentSession.LogOut(),
                    _ => () => { Console.WriteLine("Ogiltigt val."); Thread.Sleep(1000); }
                };
                action();

                if (selection == AdminMenu.Utloggning) return;
            }
        }
        internal static void CategoryManager(MyDbContext db)
        {
            while (true)
            {
                Console.Clear();
                UserInterface.DisplayLogo();
                Console.WriteLine("---Kategorihantering---");
                Console.WriteLine();
                Console.WriteLine("[L]Lägg till kategori");
                Console.WriteLine("[T]Ta bort kategori");
                Console.WriteLine("[Q]Tillbaka");

                ConsoleKey key = Console.ReadKey(true).Key;

                Action action = key switch
                {
                    ConsoleKey.L => () => EFRepository.AddCategory(db),
                    ConsoleKey.T => () => EFRepository.RemoveCategory(db),
                    ConsoleKey.Q => () => { }
                    ,
                    _ => () => { Console.WriteLine("Ogiltigt val."); Thread.Sleep(1000); }
                };
                action();
                if (key == ConsoleKey.Q) return;
            }
        }
        internal static void ProductManager(MyDbContext db)
        {
            while (true)
            {
                Console.Clear();
                UserInterface.DisplayLogo();
                Console.WriteLine("---Produkthantering---");
                Console.WriteLine();
                Console.WriteLine("[L]Lägg till Produkt");
                Console.WriteLine("[V]Ändra visibilitet");
                Console.WriteLine("[R]Redigera Produkt");
                Console.WriteLine("[Q]Tillbaka");

                ConsoleKey key = Console.ReadKey(true).Key;

                Action action = key switch
                {
                    ConsoleKey.L => () => EFRepository.AddProduct(db),
                    ConsoleKey.V => () => EFRepository.ProductVisibility(db),
                    ConsoleKey.R => () => EFRepository.EditProduct(db),
                    ConsoleKey.Q => () => { }
                    ,
                    _ => () => { Console.WriteLine("Ogiltigt val."); Thread.Sleep(1000); }
                };
                action();
                if (key == ConsoleKey.Q) return;
            }
        }

        internal static void CustomerManager(MyDbContext db)
        {

        }
        internal static async Task Statistics(MyDbContext db)
        {
            Console.Write("\u001bc\x1b[3J");
            var sw = Stopwatch.StartNew();
            var topSearch = await MongoRepository.TopSearchAsync();
            sw.Stop();
            Console.Write("\u001bc\x1b[3J");
            Console.WriteLine("---TOP SÖKNINGAR---");
            Console.WriteLine();
            foreach (var s in topSearch)
            {
                Console.WriteLine(s.search + " " + s.count);
            }
            Console.WriteLine();
            Console.WriteLine("Hämtningen från MongoDB tog" + sw.ElapsedMilliseconds + "ms.");
            Console.WriteLine();
            var connection = db.Database.GetDbConnection();
            sw.Restart();
            await DapperRepository.GetStatistics(connection);
            sw.Stop();
            Console.WriteLine();
            Console.WriteLine("Hämtningen från Dapper tog " + sw.ElapsedMilliseconds + "ms");
            Console.ReadKey(true);
            Console.WriteLine("Tryck på valfri tangent...");
        }
        internal static void OrderDelete(MyDbContext db, int orderId)
        {
            var orderToDelete = db.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefault(o => o.Id == orderId);

            if (orderToDelete != null)
            {
                db.Orders.Remove(orderToDelete);
                db.SaveChanges();
                Console.WriteLine("Order " + orderToDelete.Id + " borttagen.");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Ordern kunde inte hittas.");
                Console.ReadLine();
            }
        }
        internal static LifeCycle SetLifeCycle()
        {
            while (true)
            {
                Console.WriteLine("Ange LivsCykel");
                Console.WriteLine("[1]Annuell | [2]Bienn | [3]Perenn");

                ConsoleKeyInfo key = Console.ReadKey(true);

                var result = key.Key switch
                {
                    ConsoleKey.D1 or ConsoleKey.NumPad1 => LifeCycle.Annual,
                    ConsoleKey.D2 or ConsoleKey.NumPad2 => LifeCycle.Biennial,
                    ConsoleKey.D3 or ConsoleKey.NumPad3 => LifeCycle.Biennial,
                    _ => LifeCycle.None
                };
                if (result != LifeCycle.None)
                {
                    return result;
                }
                else
                {
                    Console.WriteLine("Du måste ange ett giltigt svar");
                }
            }
        }
        internal static Water SetWater()
        {
            while (true)
            {
                Console.WriteLine("Ange vattning");
                Console.WriteLine("[1]Mindre ofta | [2]Medel | [3]Ofta");

                ConsoleKeyInfo key = Console.ReadKey(true);

                var result = key.Key switch
                {
                    ConsoleKey.D1 or ConsoleKey.NumPad1 => Water.Low,
                    ConsoleKey.D2 or ConsoleKey.NumPad2 => Water.Medium,
                    ConsoleKey.D3 or ConsoleKey.NumPad3 => Water.High,
                    _ => Water.None
                };
                if (result != Water.None)
                {
                    return result;
                }
                else
                {
                    Console.WriteLine("Du måste ange ett giltigt svar");
                }
            }
        }
        internal static Sunlight SetSunLight()
        {
            while (true)
            {
                Console.WriteLine("Ange solmängd");
                Console.WriteLine("[1]Skugga | [2]Medel skugga | [3]Full sol");

                ConsoleKeyInfo key = Console.ReadKey(true);

                var result = key.Key switch
                {
                    ConsoleKey.D1 or ConsoleKey.NumPad1 => Sunlight.Shade,
                    ConsoleKey.D2 or ConsoleKey.NumPad2 => Sunlight.MediumShade,
                    ConsoleKey.D3 or ConsoleKey.NumPad3 => Sunlight.FullSun,
                    _ => Sunlight.None
                };
                if (result != Sunlight.None)
                {
                    return result;
                }
                else
                {
                    Console.WriteLine("Du måste ange ett giltigt svar");
                }
            }
        }
        internal static void OnDisplayAdmin(MyDbContext db)
        {
            var allProducts = db.Products.ToList();
            foreach (var product in allProducts)
            {
                Console.WriteLine($"{product.Id,-4} | {product.Name}");
                product.IsOnDisplay = false;
            }
            int count = 0;
            while (count < 3)
            {
                int selectedId = TextHelpers.GetIntInput($"Ange id {count + 1} av 3: ");
                var selectedProduct = db.Products.FirstOrDefault(x => x.Id == selectedId);
                if (selectedProduct != null)
                {
                    if (!selectedProduct.IsOnDisplay)
                    {
                        selectedProduct.IsOnDisplay = true;
                        count++;
                        Console.WriteLine(selectedProduct.Name + " tillagd");
                    }
                    else
                    {
                        Console.WriteLine(selectedProduct.Name + " Ligger redan i listan");
                    }
                }
                else
                {
                    Console.WriteLine("Kunde inte hitta produkten");
                }

            }
            db.SaveChanges();
        }
        public static void EditProductCategories(MyDbContext db, Product product)
        {
            Console.Clear();
            Console.WriteLine("---REDIGERA PRODUKT KATEGOIER---\"" + product.Name + "\"---");
            Console.WriteLine();
            var allCategories = db.Categories.ToList();
            foreach (var category in allCategories)
            {
                Console.WriteLine($"{category.Id,-4} | {category.Name}");
            }
            product.Categories.Clear();
            Console.WriteLine("Välj minst 3 kategoriId att knyta till produken, efter det kan du avsluta med [0]");
            while (true)
            {
                int count = product.Categories.Count;
                if (count < 3)
                {
                    Console.WriteLine($"Välj kategori {count + 1} (Minst 3)");
                }
                else
                {
                    Console.WriteLine("Välj en till kategori eller skriv 0 för att spara");
                }
                int choice = TextHelpers.GetIntInput("Ange ett Id");
                if (choice == 0)
                {
                    if (count >= 3)
                    {
                        break;
                    }
                    Console.WriteLine("Du måste ange minst 3 Id");
                    Thread.Sleep(1000);
                    continue;
                }
                var category = allCategories.FirstOrDefault(c => c.Id == choice);
                if (category == null)
                {
                    Console.WriteLine("Gilltigt Id måste anges");
                }
                else if (product.Categories.Contains(category))
                {
                    Console.WriteLine("Kategorin finns redan knuten till produkten");
                }
                else
                {
                    product.Categories.Add(category);
                    Console.WriteLine(category.Name + " tillagd");
                }
            }
        }
        internal static void ChangeWholesale(MyDbContext db, Product product)
        {
            while (true)
            {
                Console.WriteLine("---ÄNDRA GROSSIST---");
                Console.WriteLine();
                var allWholesalers = db.Wholesalers.ToList();
                foreach (var seller in allWholesalers)
                {
                    Console.WriteLine($"{seller.Id,-4} | {seller.Name}");
                }


                int choice = TextHelpers.GetIntInput("Välj ett Id eller 0 för att avsluta");
                if (choice == 0) return;

                var wholeseller = db.Wholesalers.FirstOrDefault(w => w.Id == choice);
                if (wholeseller != null)
                {
                    product.Wholesaler = wholeseller;
                    Console.WriteLine("Grossist ändrad");
                    Thread.Sleep(1000);
                    return;
                }
                else
                {
                    Console.WriteLine("Grossit ej funnen");
                    Thread.Sleep(1000);
                }

            }
        }
        internal static void EditCustomer(MyDbContext db)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("--SÖK KUND---");

                string email = TextHelpers.RequiredField("Ange kundens email (eller 'Q' för meny): ");
                if (email.ToLower() == "q") break;

                var customer = EFRepository.GetCustomerWithHistory(db, email);
                if (customer == null)
                {
                    Console.WriteLine("Kunden hittades inte.");
                    Thread.Sleep(1000);
                    continue;
                }

                bool inMenu = true;
                while (inMenu)
                {
                    Console.Clear();
                    UserInterface.CustomerInfo(customer);
                    UserInterface.CustomerOrders(customer);

                    Console.WriteLine("\n[N]Namn \n| [A]Adress \n| [L]Nyhetsbrev \n| [P]Lösenord  \n| [O]Ordrar");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("| [R]RADERA");
                    Console.ResetColor();
                    Console.WriteLine("| [Q]Klar/Spara");
                    var key = Console.ReadKey(true).Key;

                    Action action = key switch
                    {
                        ConsoleKey.N => () => customer.CustomerName = TextHelpers.RequiredField("Nytt namn"),
                        ConsoleKey.A => () =>
                        {
                            customer.CustomerAdress = TextHelpers.RequiredField("Adress");
                            customer.ZipCode = TextHelpers.RequiredField("Postnr");
                            customer.CustomerCity = TextHelpers.RequiredField("Stad");
                        },
                        ConsoleKey.L => () => customer.CustomerNewsLetter = !customer.CustomerNewsLetter,
                        ConsoleKey.P => () =>
                        {
                            Console.WriteLine();
                            Console.WriteLine($"Simulerar utskick av återställningslänk till: {customer.CustomerEmail}...");
                            Thread.Sleep(1500); 
                            Console.WriteLine("E-post skickat! Kunden återställer lösenordet själv.");
                            Thread.Sleep(1500);
                        },
                        ConsoleKey.O => () =>
                        {
                            int orderId = TextHelpers.GetIntInput("Ange Order-ID att hantera (0 för avbryt): ");
                            if (orderId > 0)
                            {
                                var order = customer.Orders.FirstOrDefault(o => o.Id == orderId);
                                if (order != null) EditCustomerOrders(db, customer);
                                else { Console.WriteLine("Ordern hittades inte."); Thread.Sleep(1000); }
                            }
                        }
                        ,
                        ConsoleKey.R => () =>
                        {
                            if (TextHelpers.YesNoReturn("Vill du verkligen RADERA kunden?"))
                            {
                                foreach (var o in customer.Orders) o.CustomerId = null;
                                db.Customers.Remove(customer);
                                db.SaveChanges();
                                inMenu = false;
                            }
                        },
                        ConsoleKey.Q => () =>
                        {
                            if (TextHelpers.YesNoReturn("Spara ändringar?"))
                            {
                                db.SaveChanges();
                                Console.WriteLine("Allt sparat!");
                                Thread.Sleep(1000);
                            }
                            inMenu = false;
                        }
                        ,
                        _ => () => { }
                    };

                    action();
                }
            }
        }

        internal static void EditCustomerOrders(MyDbContext db, Customer customer)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"--- ORDERHISTORIK: {customer.CustomerName.ToUpper()} ---");

                var orderList = customer.Orders.OrderByDescending(o => o.OrderDate).ToList();
                if (!orderList.Any())
                {
                    Console.WriteLine("Kunden har inga ordrar.");
                    Thread.Sleep(1500);
                    return;
                }

                for (int i = 0; i < orderList.Count; i++)
                {
                    var order = orderList[i];
                    Console.Write($"[{i + 1}] Order #{order.Id} - {order.OrderDate:yyyy-MM-dd}");

                    if (order.IsCanceld)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" (RETURNERAD)");
                        Console.ResetColor();
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
                Console.WriteLine("Välj index för detaljer eller [0] för att gå tillbaka");
                int choice = TextHelpers.GetIntInput("Val: ");

                if (choice == 0) return;

                if (choice > 0 && choice <= orderList.Count)
                {
                    var selectedOrder = orderList[choice - 1];
                    bool viewOrder = true;

                    while (viewOrder)
                    {
                        Console.Clear();
                        Console.WriteLine($"--- HANTERA ORDER #{selectedOrder.Id} ---");


                        foreach (var line in selectedOrder.OrderItems)
                        {
                            Console.WriteLine($" - {line.ProductName} ({line.ProductQuantity} st)");
                        }

                        Console.WriteLine("\n[R] ÅTERKALLA / RETURNERA");
                        Console.WriteLine("[Q] TILLBAKA TILL LISTAN");

                        var key = Console.ReadKey(true).Key;
                        switch (key)
                        {
                            case ConsoleKey.R:
                                if (selectedOrder.IsCanceld)
                                {
                                    Console.WriteLine("\nOrdern är redan returnerad.");
                                    Thread.Sleep(1000);
                                }
                                else if (TextHelpers.YesNoReturn("Vill du återkalla ordern och återställa lagret?"))
                                {
                                    foreach (var line in selectedOrder.OrderItems)
                                    {
                                        var product = db.Products.Find(line.ProductId);
                                        if (product != null)
                                        {
                                            product.InStock += line.ProductQuantity;
                                        }
                                    }
                                    selectedOrder.IsCanceld = true;
                                    db.SaveChanges();
                                    Console.WriteLine("\nOrder återkallad!");
                                    Thread.Sleep(1000);
                                }
                                viewOrder = false;
                                break;

                            case ConsoleKey.Q:
                                viewOrder = false;
                                break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltigt val.");
                    Thread.Sleep(1000);
                }
            }
        }
        internal static void StockSaldo(MyDbContext db)
        {
            Console.Clear();
            var products = db.Products.OrderBy(p => p.InStock).ToList();

            foreach (var product in products)
            {
                if (product.InStock <= 10)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if (product.InStock < 25)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                Console.WriteLine("ID: " + product.Id + " | Namn: " + product.Name + " | Antal: " + product.InStock);

                Console.ResetColor();
             
            }
            Console.ReadKey(true);
        }
    }
}


