using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WebShop.Data
{
    public class DapperRepository
    {
        public static async Task GetStatistics(IDbConnection connection)
        {
            string sql = @"
            SELECT 
                (SELECT COUNT(*) FROM Products) as TotalProducts,
                
                (SELECT AVG(Price) FROM Products) as AveragePrice,
                
                (SELECT COUNT(*) FROM Categories) as TotalCategories,
                
                (SELECT SUM(Price * InStock) FROM Products) as TotalInventoryValue,
                
                (SELECT MAX(Price) FROM Products) as MostExpensivePrice,
                
                (SELECT TOP 1 c.Name FROM Categories c 
                JOIN ProductCategory pc ON c.Id = pc.CategoriesId 
                GROUP BY c.Name 
                ORDER BY COUNT(c.Id) DESC) as TopCategory,

                (SELECT COUNT(*) FROM Customers) as TotalCustomers,

                (SELECT TOP 1 c.CustomerName FROM Customers c
                JOIN Orders o ON c.Id = o.CustomerId
                GROUP BY c.CustomerName
                ORDER BY SUM(o.TotalOrderPrice) DESC) as BestCustomer,

                (SELECT TOP 1 CustomerCity FROM Customers
                GROUP BY CustomerCity
                ORDER BY COUNT(*) DESC) as TopCity,

                (SELECT COUNT(*) FROM Customers WHERE CustomerNewsLetter = 1) as Subscribers,

                (SELECT COUNT(*) FROM Orders) as TotalOrders";

            var stats = await connection.QuerySingleAsync(sql);
            
            Console.WriteLine("---BUTIKSSTATISTIK---");
            Console.WriteLine(new string('-', 40));
            Console.WriteLine($"| {"Antal unika produkter:",-27} {stats.TotalProducts}");
            Console.WriteLine($"| {"Antal kategorier:",-27} {stats.TotalCategories}");
            Console.WriteLine($"| {"Snittpris på vara:",-27} {stats.AveragePrice / 100m:C}");
            Console.WriteLine($"| {"Totalt lagervärde:",-27} {stats.TotalInventoryValue / 100m:C}");
            Console.WriteLine($"| {"Dyrast i butiken:",-27} {stats.MostExpensivePrice / 100m:C}");
            Console.WriteLine($"| {"Största kategori:",-27} {stats.TopCategory}");
            Console.WriteLine();
            Console.WriteLine("---KUNDSTATISTIK---");
            Console.WriteLine(new string('-', 40));
            Console.WriteLine($"| {"Totalt antal kunder:",-27} {stats.TotalCustomers}");
            Console.WriteLine($"| {"Bästa kund:",-27} {stats.BestCustomer}");
            Console.WriteLine($"| {"Vanligaste stad:",-27} {stats.TopCity}");
            Console.WriteLine($"| {"Nyhetsbrev:",-27} {stats.Subscribers}");
            Console.WriteLine($"| {"Totalt antal ordrar:",-27} {stats.TotalOrders}");
        }
    }
}

