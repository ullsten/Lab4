using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Lab4_Induvidual_Database_Project.Data;
using Labb4_Individual_Database_project;
using Spectre.Console;

namespace Lab4_Induvidual_Database_Project
{
    public class LoginService
    {
        public void LoginSchool()
        {
            Menu menu = new Menu();
            HashPassword hash = new HashPassword();
            using (var context = new SchoolContext())
            {
                var username = "";
                var password = "";
                var hashDB = ""; //stores hashed password from database to compare with input password

                var ValidatePassword = from u in context.UserInfos
                                       where u.Username == username
                                       select u;
                Console.ForegroundColor= ConsoleColor.Green;
                AnsiConsole.Markup("Enter username: ");
                Console.ResetColor();
                username = Console.ReadLine();
                Console.Write("Enter your password: ");
                var passInput = Console.ReadLine();
                foreach (var item in ValidatePassword) { hashDB = item.HashedPassword; } //Get stored hashedpassword to hashDB
                while (true)
                {
                    passInput = hash.hashPassword(passInput); //get hash for input password compare with stored hashpassword
                    if (passInput.Equals(hashDB))
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Thread.Sleep(500);
                        Console.WriteLine("_");
                        Thread.Sleep(500);
                        Console.WriteLine(" _");
                        Thread.Sleep(500);
                        Console.WriteLine("-");
                        Console.ForegroundColor= ConsoleColor.Yellow;
                        Console.WriteLine("Login succeeded, your choices are coming!");
                        Thread.Sleep(1500);
                        Console.ResetColor();
                        menu.AdminMenu();
                        break;
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Invalid password! Try again![/]");
                        passInput = Console.ReadLine();
                    }
                }
            }
        }
    }
}
