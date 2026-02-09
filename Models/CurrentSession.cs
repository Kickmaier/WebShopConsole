using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Models
{
    internal static class CurrentSession
    {
        public static Customer? CurrentCustomer { get; set; }
        public static bool isLoggedIn => CurrentCustomer != null;

        public static void LogOut()
        {
            CurrentCustomer = null;
        }
    }
}
