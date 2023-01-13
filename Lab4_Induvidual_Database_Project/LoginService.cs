using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Lab4_Induvidual_Database_Project.Data;
using Labb4_Individual_Database_project;
using Spectre.Console;
using PanoramicData.ConsoleExtensions;
using System.ComponentModel;
using System.Diagnostics;

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
                AnsiConsole.MarkupLine("[blue]Login is required[/]");
                Console.WriteLine();
                Console.ForegroundColor= ConsoleColor.Green;
                AnsiConsole.Markup("Username: ");
                username = Console.ReadLine();
                Console.Write("Password: ");
                Console.ResetColor();
                var passInput = ConsolePlus.ReadPassword(); //secure password
                Console.WriteLine();
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
        public void SecureString()
        {
            ConsoleKeyInfo cki;
            String m = "\nEnter your password (up to 15 letters, numbers, and underscores)\n" +
                       "Press BACKSPACE to delete the last character entered. " +
                       "\nPress Enter when done, or ESCAPE to quit:";
            SecureString password = new SecureString();
            int top, left;

            // The Console.TreatControlCAsInput property prevents CTRL+C from
            // ending this example.
            Console.TreatControlCAsInput = true;

            Console.Clear();
            Console.WriteLine(m);

            top = Console.CursorTop;
            left = Console.CursorLeft;

            // Read user input from the console. Store up to 15 letter, digit, or underscore
            // characters in a SecureString object, or delete a character if the user enters
            // a backspace. Display an asterisk (*) on the console to represent each character
            // that is stored.

            do
            {
                cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Escape) break;

                if (cki.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        Console.SetCursorPosition(left + password.Length - 1, top);
                        Console.Write(' ');
                        Console.SetCursorPosition(left + password.Length - 1, top);
                        password.RemoveAt(password.Length - 1);
                    }
                }
                else
                {
                    if ((password.Length < 15) &&
                         (Char.IsLetterOrDigit(cki.KeyChar) || cki.KeyChar == '_'))
                    {
                        password.AppendChar(cki.KeyChar);
                        Console.SetCursorPosition(left + password.Length - 1, top);
                        Console.Write('¨');
                    }
                }
            } while (cki.Key != ConsoleKey.Enter & password.Length < 15);

            // Make the password read-only to prevent modification.
            password.MakeReadOnly();
            // Dispose of the SecureString instance.
            password.Dispose();
        }
    }
}
