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
        public void AddUserInfo() //Add new userinfo to database
        {
            using (var context = new SchoolContext())
            {
                var newUserName = "";
                var newPassword = "";

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
        public string hashPassword(string password) //Create new hashed password
        {
            var sha = SHA512.Create();
            var asByteArray = Encoding.Default.GetBytes(password);
            var hashedPassword = sha.ComputeHash(asByteArray);
            return Convert.ToBase64String(hashedPassword);
        }
    }
}      

            
        
    
   

