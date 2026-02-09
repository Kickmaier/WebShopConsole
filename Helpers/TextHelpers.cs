using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebShop.Helpers
{
    internal class TextHelpers
    {
        internal const int productWidth = 28;
        internal static void PrintCentered(List<string> rows)
        {
            int windowWidth = Console.WindowWidth;
            foreach (var line in rows)
            {
                int padding = (windowWidth - line.Length) / 2;
                Console.WriteLine(new string(' ', Math.Max(0, padding)) + line);
            }
        }
        internal static bool YesNoReturn(string field)
        {
            while (true)
            {
                Console.WriteLine(field + " J/N");
                string? input = Console.ReadLine()?.ToUpper();

                if (input == "J" || input == "j")
                {
                    return true;
                }
                else if (input == "N" || input == "n")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Felaktig inmatning");
                    Thread.Sleep(1000);
                }
            }
        }
        internal static string RequiredField(string field)
        {
            while (true)
            {
                Console.WriteLine(field);
                string input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    return input;
                }
                else
                {
                    Console.WriteLine("Fältet måste vara ifyllt för att fortsätta");
                    Thread.Sleep(1000);
                }
            }
        }
        internal static int GetIntInput(string input)
        {
            int value;
            Console.Write(input);
            while (!int.TryParse(Console.ReadLine(), out value))
            {
                Console.WriteLine("Felaktig inmatning, ange ett heltal.");
                Console.Write(input);
            }
            return value;
        }
        internal static string PasswordCreation()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("---SKAPA LÖSENORD---");
                Console.WriteLine("Välj ett lösenord");
                string input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Du måste ange ett lösenord");
                    Thread.Sleep(2000);
                    continue;
                }
                Console.WriteLine("Ange lösenordet igen");
                string check = Console.ReadLine();
                if (input == check)
                {
                    return check;
                }
                else
                {
                    Console.WriteLine("Båda fälten måste vara identiska. Försök igen");
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
