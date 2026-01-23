using WebShop.Data;
using WebShop.Services;
namespace WebShop
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using (var db = new Data.MyDbContext())
            {
                DbInitializer.Initializer(db);
                UserInterface.Startsida();
            }
        }
    }
}
