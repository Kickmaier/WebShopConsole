using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop.Helpers
{
    internal class Security
    {
        internal static bool IsAdmin => CurrentSession.CurrentCustomer?.Role == Role.Admin;
    
    public static bool AuthorizeAdmin()
        {
            if (IsAdmin) return true;

            Console.WriteLine("Fel: Du måste vara administratör.");
            Thread.Sleep(2000);
            return false;
        }
    }
}
