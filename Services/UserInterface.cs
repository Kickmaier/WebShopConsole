using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Models;
using WebShop.Data;
using WebShop.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace WebShop.Services;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Identity.Client;
using MongoDB.Bson.IO;
using WebShop.Helpers;
using Microsoft.EntityFrameworkCore;


internal class UserInterface
{
    internal static async Task Start(MyDbContext db)
    {
        await StartInteractions(db); 
    }
    internal static async Task DisplayCategories(MyDbContext db)
    {
        while (true)
        {
            Console.Clear();
            Category? selectedCategory = null;
            var categoryList = await db.Categories.ToListAsync();
            Console.WriteLine();
            for (int i = 0; i <= categoryList.Count - 1; i++)
            {
                Category c = categoryList[i];
                Console.WriteLine($"{(i + 1),-4}{c.Name}");
            }
            Console.WriteLine(new string('-', 20));
            Console.WriteLine("Välj en kategori (index eller namn)");
            string input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                return;
            }
            if (int.TryParse(input, out int index))
            {
                if ((index > 0) && (index <= categoryList.Count))
                {
                    var indexCategory = categoryList[index - 1];
                    selectedCategory = await EFRepository.GetCategoryWithProductsAsync(db, indexCategory.Name);
                }
            }
            else
            {
                selectedCategory = await EFRepository.GetCategoryWithProductsAsync(db, input);
            }
            if (selectedCategory != null)
            {
                await DisplayProductsInCategory(db, selectedCategory);
            }
            else
            {
                Console.WriteLine(input + "hittades inte. Tryck valfri tangent för att försöka igen!");
                Thread.Sleep(1000);
            }
        }
    }
    internal static void DisplayLogo()
    {
        int width = 28;
        string logo = "Nullable Garden".ToUpper();
        string moto = "--Nullable by Default!--".ToUpper();
        var logoList = new List<(string text, Align position)>();
        logoList.Add((logo, Align.Center));
        logoList.Add((moto, Align.Center));
        TextHelpers.PrintCentered(AutoFrame(logoList, width));
        Console.WriteLine(" Användare: " + (CurrentSession.CurrentCustomer != null ? CurrentSession.CurrentCustomer.CustomerName : "Gäst"));
        Console.WriteLine(new string('═', Console.WindowWidth));

    }
    internal static List<string> AutoFrame(List<(string text, Align align)> list, int width)
    {
        List<string> frameList = new List<string>();

        string roof = $"╔{new string('═', width)}╗";
        string floor = $"╚{new string('═', width)}╝";
        frameList.Add(roof);
        foreach (var row in list)
        {
            string currentRow = row.text;
            if (string.IsNullOrEmpty(currentRow))
            {
                frameList.Add($"║{new string(' ', width)}║");
                continue;
            }
            while (currentRow.Length > 0)
            {
                string remainingRow = "";
                if (currentRow.Length <= width)
                {
                    remainingRow = currentRow;
                    currentRow = "";
                }
                else
                {
                    int wrapIndex = currentRow.LastIndexOf(' ', width);
                    if (wrapIndex <= 0)
                    {
                        int cut = width - 1;
                        remainingRow = currentRow.Substring(0, cut) + "-";
                        currentRow = currentRow.Substring(cut);
                    }
                    else
                    {
                        remainingRow = currentRow.Substring(0, wrapIndex);
                        currentRow = currentRow.Substring(wrapIndex).TrimStart();
                    }
                }
                if (row.align == Align.Center)
                {
                    int padding = (width - remainingRow.Length) / 2;
                    string leftPad = new string(' ', padding);
                    string rightPad = new string(' ', width - remainingRow.Length - padding);
                    frameList.Add($"║{leftPad}{remainingRow}{rightPad}║");
                }
                else if (row.align == Align.Right)
                {
                    string padding = new string(' ', width - remainingRow.Length - 1);
                    frameList.Add($"║{padding}{remainingRow} ║");
                }
                else
                {
                    frameList.Add($"║ {remainingRow.PadRight(width - 1)}║");
                }
            }

        }
        frameList.Add(floor);
        return frameList;
    }
    internal static async Task DisplayProductsInCategory(MyDbContext db, Category category)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"--- KATEGORI: {category.Name.ToUpper()} ---");
            Console.WriteLine();
            var productList = await db.Products
                                .Where(p => p.Categories.Any(c => c.Id == category.Id))
                                .ToListAsync();

            if (productList.Count == 0)
            {
                Console.WriteLine("Kategorin tom");
                Console.ReadKey();
                return;
            }
            Console.WriteLine($"{"PRODUKT",-20} | {"PRIS",2}");
            for (int i = 0; i < productList.Count; i++)
            {
                Product p = productList[i];
                Console.WriteLine($"{(i + 1),-4}{p.Name,-20} | {p.GetSkrPrice(),2}");
            }

            Console.WriteLine(new string('-', 30));
            Console.WriteLine("Välj en produkt (index eller namn) för att se detaljer:");

            string input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                return;
            }
            Product? selectedProduct = null;
            if (int.TryParse(input, out int index))
            {
                if (index > 0 && index <= productList.Count)
                {
                    selectedProduct = productList[index - 1];
                }
            }
            else
            {
                selectedProduct = productList.FirstOrDefault(p => p.Name.ToLower() == input.ToLower());
            }
            if (selectedProduct != null)
            {
                var fullProduct = await EFRepository.GetSingleProductAsync(db, selectedProduct.Id);

                if (fullProduct != null)
                {
                    await ProductInteraction(fullProduct, db);
                }
            }
            else
            {
                Console.WriteLine("Ingen produkt hittad");
                Console.ReadKey();
            }
        }
    }
    internal static List<string> GetProductContent(Product product)
    {
        int width = TextHelpers.productWidth;
        string line = new string('-', width - 2);
        var productToList = new List<(string text, Align align)>();
        productToList.Add(("", Align.Left));
        productToList.Add((product.Name, Align.Center));
        productToList.Add((product.ScientificName, Align.Center));
        productToList.Add((line, Align.Left));
        productToList.Add(("---Beskrivning---", Align.Center));
        productToList.Add((product.Description, Align.Left));
        productToList.Add((product.GetSkrPrice(), Align.Left));
        productToList.Add(("", Align.Left));
        productToList.Add(("Sol: " + product.GetSweSun(), Align.Left));
        productToList.Add(("Vatten: " + product.GetSweWater(), Align.Left));
        productToList.Add((product.GetSweLifecycle(), Align.Left));
        productToList.Add(("I lager: " + product.InStock, Align.Left));
        productToList.Add(("---Kategorier---", Align.Center));
        foreach (var category in product.Categories)
        {
            productToList.Add((category.Name, Align.Left));
        }
        return AutoFrame(productToList, width);
    }
    internal static void DisplaySingleProduct(Product product)
    {

        var display = GetProductContent(product);

        foreach (var row in display)
        {
            Console.WriteLine(row);
        }
    }
    internal static void OnDisplay(List<Product> products)
    {
        if (products.Count == 3)
        {
            int width = TextHelpers.productWidth; 
            var list1 = GetProductContent(products[0]);
            var list2 = GetProductContent(products[1]);
            var list3 = GetProductContent(products[2]);

            var displayList = new List<List<string>> { list1, list2, list3 };
            int maxRows = new[] { list1.Count, list2.Count, list3.Count }.Max();
            while (list1.Count < maxRows)
            {
                list1.Add(new string(' ', width+2));
            }
            while (list2.Count < maxRows)
            {
                list2.Add(new string(' ', width+2));
            }
            while (list3.Count < maxRows)
            {
                list3.Add(new string(' ', width + 2));
            }

            Console.Clear();
            DisplayLogo();
            string space = new string(' ', width + 3);
            Console.WriteLine(" ERBJUDANDEN:");
            Console.WriteLine($" A{space}B{space}C");
            for (int i = 0; i < maxRows; i++)
            {
                Console.WriteLine(list1[i] + "  " + list2[i] + "  " + list3[i]);
            }
        }
        else
        {
            Console.WriteLine("En eller flera produkter hittades inte i databasen!");
            Console.ReadKey();
        }
    }
    internal static async Task ViewCart(MyDbContext db)
    {
        string customer = CurrentSession.CurrentCustomer == null ? "Guest" : CurrentSession.CurrentCustomer.CustomerName;
        while (true)
        {
            Console.Clear();
            var cartItems = await EFRepository.GetCartAsync(db);
            if (cartItems.Count == 0)
            {
                Console.WriteLine("Kundkorgen är tom, du skickas tillbaka...");
                Thread.Sleep(1500);
                return;
            }
            else
            {
                int width = 50;
                int totalPrice = 0;
                var lines = new List<(string text, Align align)>();
                lines.Add(("Kundkorg", Align.Center));
                lines.Add((new string('-', width), Align.Center));
                for (int i = 0; i < cartItems.Count; i++)
                {
                    int rowPrice = (cartItems[i].Product.Price * cartItems[i].Quantity);
                    totalPrice += rowPrice;
                    string row = $"{i + 1}| {cartItems[i].Product.Name,-15} | {cartItems[i].Quantity,3}st | {cartItems[i].Product.GetSkrPrice(),-8})";
                    lines.Add((row, Align.Left));
                }

                string stringTotalPrice = (totalPrice / 100m).ToString("C");
                lines.Add(("---Totalsumma---", Align.Center));
                lines.Add((stringTotalPrice, Align.Right));

                var framedCart = AutoFrame(lines,width);
                foreach (var line in framedCart)
                {
                    Console.WriteLine(line);
                }
                Console.WriteLine();
                Console.WriteLine("| [nr]För att gå till produkt \n| [B]Betala \n| [Q]Tillbaka");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    continue;
                }
                string inputToUpper = input.ToUpper();
                if (inputToUpper == "Q")
                {
                    return;
                }
                else if (inputToUpper == "B")
                {
                    await MakeOrder(db, cartItems);
                    return;
                }
                else if (int.TryParse(inputToUpper, out int productChoice))
                {
                    int index = productChoice - 1;
                    if (index >= 0 && index < cartItems.Count)
                    {
                        var item = cartItems[index];
                        Console.WriteLine(item.Product.Name + "vald");
                        Console.WriteLine("[Enter]Gå till produkt | [+]Öka antal | [-]Minska antal | [R]Tabort produkt");
                        ConsoleKey key = Console.ReadKey(true).Key;
                        string name = item.Product.Name;
                        int quantity = item.Quantity;
                        Func<Task> action = key switch
                        {
                            ConsoleKey.Enter => async () => await ProductInteraction(item.Product, db),
                            ConsoleKey.Add or ConsoleKey.OemPlus => async () =>
                            {
                                await EFRepository.AddCartItemAsync(db, item.ProductId);
                                await MongoRepository.ActivityLoggerAsync("AddToCart", name, customer, +1);
                            },
                            ConsoleKey.Subtract or ConsoleKey.OemMinus => async () => 
                            {
                                await EFRepository.RemoveCartItemAsync(db, item.ProductId);
                                await MongoRepository.ActivityLoggerAsync("RemoveFromCart", name, customer, -1);
                            },
                            ConsoleKey.R => async () =>
                            {
                                await EFRepository.DeleteFromCartAsync(db, item.ProductId);
                                await MongoRepository.ActivityLoggerAsync("RemoveFromCart", name, customer, -quantity);
                            }
                            ,
                            _ => async() => { Console.WriteLine("Ogiltig inmatning!");
                                await Task.Delay(1000); },
                        };
                        await action();
                        if(key == ConsoleKey.Q) return;
                        continue;
                    }
                }
            }
        }
    }
    internal static async Task MakeOrder(MyDbContext db, List<CartItem> cartItems)
    {

        var newOrder = new Order();
        if (CurrentSession.isLoggedIn)
        {
            newOrder.CustomerId = CurrentSession.CurrentCustomer.Id;
            newOrder.CustomerName = CurrentSession.CurrentCustomer.CustomerName;
            newOrder.StreetAdress = CurrentSession.CurrentCustomer.CustomerAdress;
            newOrder.ZipCode = CurrentSession.CurrentCustomer.ZipCode;
            newOrder.City = CurrentSession.CurrentCustomer.CustomerCity;
            newOrder.CustomerEmail = CurrentSession.CurrentCustomer.CustomerEmail;
            newOrder.Newsletter = CurrentSession.CurrentCustomer.CustomerNewsLetter;
            newOrder.OrderDate = DateTime.Now;
        }
        else
        {
            Console.WriteLine("---SKAPA ORDER---");
            Console.WriteLine("\nFyll i informationen nedan för att skapa order");
            Console.WriteLine();

            newOrder.CustomerName = TextHelpers.RequiredField("Fullständiga för och efternamn ");
            newOrder.StreetAdress = TextHelpers.RequiredField("Gatuadress: ");
            newOrder.ZipCode = TextHelpers.RequiredField("Postnr: ");
            newOrder.City = TextHelpers.RequiredField("Stad: ");
            newOrder.CustomerEmail = TextHelpers.RequiredField("Ange mail (för avisering)");
            newOrder.Newsletter = TextHelpers.YesNoReturn("Vill du ha vårat fantastiska nyhetsbrev?");
            newOrder.OrderDate = DateTime.Now;
        }
        foreach (var item in cartItems)
        {
            newOrder.OrderItems.Add(new OrderItem(item.ProductId, item.Product.Name, item.Quantity, item.Product.Price));
        }
        newOrder.TotalOrderPrice = newOrder.OrderItems.Sum(x => x.PriceAtPurchase * x.ProductQuantity);


        Console.Clear();
        Console.WriteLine("Granska din order!");
        Console.WriteLine();
        Console.WriteLine($"Namn: {newOrder.CustomerName}");
        Console.WriteLine($"Adress: {newOrder.StreetAdress}\n\t{newOrder.ZipCode} {newOrder.City}");
        Console.WriteLine($"Email: {newOrder.CustomerEmail}");
        Console.WriteLine($"Nyhetsbrev: {newOrder.NewsLetterText}");
        Console.WriteLine();
        Console.WriteLine($"Order: {"Produkt",-30}{"Pris",5}{"Antal",15}");
        foreach (var item in newOrder.OrderItems)
        {
            Console.WriteLine($"\t {item.ProductName,-30}{(((item.PriceAtPurchase * item.ProductQuantity) / 100m)).ToString("c"),5} {item.ProductQuantity,15}");

        }
        Console.WriteLine();
        Console.WriteLine($"{"Varubetalning (exkl. moms):",-40} {newOrder.GetPriceExVat(),15}");
        Console.WriteLine($"{"Moms (25%):",-40} {newOrder.GetVatPrice(),15}");
        Console.WriteLine("TOTALSUMMA: " + newOrder.GetSkrPrice());

        bool confirm = TextHelpers.YesNoReturn("Stämmer informatonen så för att gå vidare till betalning?");
        bool payment = TextHelpers.YesNoReturn("Här skickas kunden till bankens hemsida för \nhantering av betalnings alternativ mm. blev betalningen godkänd?");
        if (confirm && payment)
        {
            foreach (var item in newOrder.OrderItems)
            {

                var product = await db.Products.FindAsync(item.ProductId);

                if (product != null)
                {
                    product.InStock -= item.ProductQuantity;
                }
            }
            if (!CurrentSession.isLoggedIn)
            {
                var existingCustomer = await db.Customers
                                        .FirstOrDefaultAsync(c => c.CustomerEmail == newOrder.CustomerEmail);
                if (existingCustomer != null)
                {
                    Console.WriteLine("Mailen finns redan i vårat system. Vill du ange lösenord för att logga in?");
                    if (TextHelpers.YesNoReturn("Vill du ange lösenord för att logga in?"))
                    {
                        string passwordCheck = TextHelpers.RequiredField("Ange lösenord");
                        if (existingCustomer.CustomerPassword == passwordCheck)
                        {
                            newOrder.CustomerId = existingCustomer.Id;
                            CurrentSession.CurrentCustomer = existingCustomer;
                            Console.WriteLine("Du är nu inloggad och din order är registrerad på ditt inlogg.");
                        }
                        else
                        {
                            Console.WriteLine("Inloggning misslyckades forsätter som gäst");
                        }
                    }
                }
                else
                {
                    if (TextHelpers.YesNoReturn("Vill du skapa ett konto"))
                    {
                        string password = TextHelpers.PasswordCreation();
                        var newCustomer = new Customer(newOrder.CustomerName, newOrder.CustomerEmail, newOrder.StreetAdress, newOrder.ZipCode, newOrder.City, password, newOrder.Newsletter);
                        db.Customers.Add(newCustomer);
                        await db.SaveChangesAsync();
                        newOrder.CustomerId = newCustomer.Id;
                    }
                }
            }
            await db.Orders.AddAsync(newOrder);
            db.CartItems.RemoveRange(cartItems);
            await db.SaveChangesAsync();
            Console.WriteLine("Order skapad och du skickas tillbaka...");
            Console.ReadLine();
            return;

        }
        else
        {
            Console.WriteLine("Order skapades ej skickas tillbaka till varukorgen");
            Console.WriteLine("Tryck på valfri tangent!");
            Console.ReadLine();
            return;
        }
    }
    internal static async Task ProductInteraction(Product product, MyDbContext db)
    {
        while (true)
        {
            Console.Clear();
            DisplaySingleProduct(product);

            Console.WriteLine("| [L]Lägg i varukorg \n| [T]Ta bort vara \n| [V]Gå till varukorg \n| [Q}Tillbaka");
            string customer = CurrentSession.CurrentCustomer == null ? "Guest" : CurrentSession.CurrentCustomer.CustomerName;
            var key = Console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.L:
                    EFRepository.AddCartItemAsync(db, product.Id);
                    Console.WriteLine(product.Name + " tillagd!");
                    await MongoRepository.ActivityLoggerAsync("AddToCart", product.Name, customer, +1);
                    Thread.Sleep(1000);
                    break;
                case ConsoleKey.T:
                    EFRepository.RemoveCartItemAsync(db, product.Id);
                    Console.WriteLine(product.Name + " borttagen");
                    await MongoRepository.ActivityLoggerAsync("RemoveFromCart", product.Name, customer, -1);
                    Thread.Sleep(1000);
                    break;
                case ConsoleKey.V:
                    await ViewCart(db);
                    return;
                case ConsoleKey.Q:
                    return;
            }
        }
    }
    internal static async Task StartInteractions(MyDbContext db)
    {
        while (true)
        {
            var onDisplayList = await EFRepository.GetOnDisplayAsync(db);

            DisplayLogo();
            OnDisplay(onDisplayList);
            Console.WriteLine("| [A]Visa produkt A \n| [B]Visa produkt B \n| [C]Visa produkt C \n| [K]Kategorier \n| [S]Sök \n| [V]Varukorg \n| [M]Medlemssidan \n| [U]Logga Ut \n| [Q]Avsluta");

            ConsoleKeyInfo key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.A:
                    await ProductInteraction(onDisplayList[0], db);
                    break;
                case ConsoleKey.B:
                    await ProductInteraction(onDisplayList[1], db);
                    break;
                case ConsoleKey.C:
                    await ProductInteraction(onDisplayList[2], db);
                    break;
                case ConsoleKey.K:
                    await DisplayCategories(db);
                    break;
                case ConsoleKey.S:
                    await SearchProduct(db);
                    break;
                case ConsoleKey.V:
                    await ViewCart(db);
                    break;
                case ConsoleKey.M:
                    await EFRepository.LoginAsync(db);
                    break;
                case ConsoleKey.U:
                    CurrentSession.LogOut();
                    break;
                case ConsoleKey.Q:
                    return;
            }
        }
    }
    internal static void FullCustomerInfo(Customer customer)
    {
        Console.Clear();
        Console.WriteLine("---KUNDINFO---");
        CustomerInfo(customer);
        if (customer.Orders == null || !customer.Orders.Any())
        {
            Console.WriteLine("Ingen orderhistorik tillgänglig.");
        }
        else
        {
            foreach (var order in customer.Orders)
            {
                Console.WriteLine($"\nORDER #{order.Id} [{order.OrderDate:yyyy-MM-dd}]");
                Console.WriteLine(new string('-', 40));

                Console.WriteLine($"{"Produkt",-20} {"Antal",-8} {"Pris",-10}");

                foreach (var item in order.OrderItems)
                {
                    Console.WriteLine($"{item.ProductName,-20} {item.ProductQuantity + " st",-8} {item.GetSkrPrice() + " kr",-10}");
                }
                Console.WriteLine(new string('-', 40));
                Console.WriteLine($"TOTALSUMMA: {order.GetSkrPrice()}");
            }
        }

        Console.WriteLine("\nTryck på valfri tangent för att gå tillbaka...");
        Console.ReadKey();
    }
    internal static void CustomerInfo(Customer customer)
    {
        Console.WriteLine(new string('=', 40));
        Console.WriteLine($"KUND:   {customer.CustomerName} (#{customer.Id})");
        Console.WriteLine($"EMAIL:  {customer.CustomerEmail}");
        Console.WriteLine($"ADRESS: {customer.CustomerAdress}, \n\t{customer.ZipCode} {customer.CustomerCity}");
        Console.WriteLine($"NYHETSBREV: {customer.NewsLetterText}");
        Console.WriteLine(new string('=', 40));
        Console.WriteLine();
    }
    internal static void CustomerOrders(Customer customer)
    {
        Console.WriteLine($"---ORDERHISTORIK: {customer.CustomerName.ToUpper()}---");

        if (!customer.Orders.Any())
        {
            Console.WriteLine("Inga ordrar registrerade.");
        }
        else
        {
            var orders = customer.Orders.OrderByDescending(o => o.OrderDate).ToList();

            Console.WriteLine($"  {"VAL",-5} {"ID",-6} {"DATUM",-12} {"STATUS"} ");
            Console.WriteLine(new string('-', 45));

            for (int i = 0; i < orders.Count; i++)
            {
                var o = orders[i];

                Console.WriteLine($"  [{i + 1,2}]  #{o.Id,-5} {o.OrderDate:yyyy-MM-dd}  ");

                if (o.IsCanceld)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("RETURNERAD");
                    Console.ResetColor();
                }
            }
        }
        Console.WriteLine(new string('-', 45));
    }
    internal static async Task SearchProduct(MyDbContext db)
    {
        Console.Clear();
        Console.WriteLine("---SÖK---");
        Console.WriteLine();
        Console.WriteLine("Ange produkt du söker efter");
        Console.WriteLine();
        string search = Console.ReadLine();
        var searchList = await EFRepository.SeachProductAsync(db, search);
        string customer = (CurrentSession.CurrentCustomer == null) ? "Guest" : CurrentSession.CurrentCustomer.CustomerName;
        await MongoRepository.ActivityLoggerAsync("search", search, customer);
        if (searchList.Count > 0)
        {
            for (int i = 0; i < searchList.Count; i++)
            {
                Console.WriteLine($"{i + 1,-4} | {searchList[i].Name}");
            }
            Console.WriteLine();
            int choice = TextHelpers.GetIntInput("Tryck 0 för att återvända eller index för att se produkt; ") - 1;
            if (choice == -1) return;
            if (choice > 0 && choice < searchList.Count)
            {
                var selectedProduct = searchList[choice];
                await ProductInteraction(selectedProduct,db);
            }
            else
            {
                Console.WriteLine("Ogiltigt val");
                Thread.Sleep(1000);
            }
        }
        else
        {
            Console.WriteLine("Inga produkter matcha din sökning");
            Thread.Sleep(1000);
        }
    }

}
