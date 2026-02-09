using WebShop.Data;
using WebShop.Models;
using WebShop.Services;
using Microsoft.EntityFrameworkCore;

namespace WebShop
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using (var db = new Data.MyDbContext())
            {
                await DbInitializer.Initializer(db);
                
                await UserInterface.Start(db);
                //Helpers.TextHelpers.ToCenter();
                //ProductManager.ProductView();
                //EFRepository.DeleteAllOrders(db);
            }
        }
    }
}
