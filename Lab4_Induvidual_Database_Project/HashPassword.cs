using System.Security.Cryptography;
using DocumentFormat.OpenXml.Office.Word;
using Lab4_Induvidual_Database_Project.Data;
using Labb4_Individual_Database_project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using BCryptNet = BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using DocumentFormat.OpenXml.InkML;
using UserInfo = Lab4_Induvidual_Database_Project.Models.UserInfo;
using System.Security.Cryptography;

namespace Lab4_Induvidual_Database_Project
{
    public class HashPassword
    {
        public void AddUserInfo() //Login user DENNNA
        {
            using (var context = new SchoolContext())
            {
                var newUserName = "";
                var newPassword = "";

                string hashPassword(string password)
                {
                    var sha = SHA512.Create();
                    var asByteArray = Encoding.Default.GetBytes(password);
                    var hashedPassword = sha.ComputeHash(asByteArray);
                    return Convert.ToBase64String(hashedPassword);
                }
                
                Console.Write("Enter username: ");
                newUserName = Console.ReadLine();
                Console.Write("Enter password: ");
                newPassword = Console.ReadLine();

                newPassword = hashPassword(newPassword);//generate new hasheh password

                var newUser = new UserInfo();
                {
                    newUser.Username = newUserName;
                    newUser.HashedPassword = newPassword;
                }
                context.Add(newUser);
                context.SaveChanges();
                Console.WriteLine();
                Console.WriteLine("User added");
            }
        }
        public void LoginSchool() //DENNA
        {
            Menu menu= new Menu();
            using (var context = new SchoolContext())
            {
                string hashPassword(string password) //generate new hashed password
                {
                    var sha = SHA512.Create();
                    var asByteArray = Encoding.Default.GetBytes(password);
                    var hashedPassword = sha.ComputeHash(asByteArray);
                    return Convert.ToBase64String(hashedPassword);
                }
                var username = "";
                var password = "";
                var hashDB = ""; //stores hashed password from database to compare with input password

                var ValidatePassword = from u in context.UserInfos
                                       where u.Username == username
                                       select u;
                Console.Write("Enter username: ");
                username = Console.ReadLine();
                Console.Write("Enter your password: ");
                var passInput = Console.ReadLine();
                foreach (var item in ValidatePassword) {hashDB = item.HashedPassword; } //Get stored hashedpassword to hashDB
                while (true)
                {
                    passInput = hashPassword(passInput); //get hash for input password to later compare with stored hashpassword
                        if (passInput.Equals(hashDB))
                        {
                            Console.WriteLine("You have been logged in.");
                        menu.AdminMenu();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid password! Try again!");
                            passInput = Console.ReadLine();
                        }
                }
                
            }

        }
    }
}      

            
        
    
   

