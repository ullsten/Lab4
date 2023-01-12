using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Lab4_Induvidual_Database_Project.Data;
using Lab4_Induvidual_Database_Project.Models;
using SixLabors.ImageSharp.ColorSpaces;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Lab4_Induvidual_Database_Project
{
    public class HashSalt
    {
        public string Hash { get; set; } //1
        public string Salt { get; set; } //1

        public static HashSalt GenerateSaltedHash(int size, string password) //Generate hash and salt
        {
            var saltBytes = new byte[size];
            var provider = new RNGCryptoServiceProvider();
            provider.GetNonZeroBytes(saltBytes);
            var salt = Convert.ToBase64String(saltBytes);

            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000);
            var hashPassword = Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256));

            HashSalt hashSalt = new HashSalt { Hash = hashPassword, Salt = salt };
            return hashSalt;
        } //1

        public static bool VerifyPassword(string enteredPassword, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(enteredPassword, saltBytes, 10000);
            return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(256)) == storedHash;
        }//1

        private static string GetRandomSalt() //BCrypt
        {
            return BCryptNet.GenerateSalt(12);
        }

        public static string HashPassword(string password) //BCrypt
        {
            return BCryptNet.HashPassword(password, GetRandomSalt());
        }

        public static bool ValidatePassword(string password, string correctHash)//BCrypt
        {
            return BCryptNet.Verify(password, correctHash);
        }
        public void CreateUserInfo() //BCrypt
        {
            using (var context = new SchoolContext())
            {
                var password = "";
                var userName = "";
                var existingHashFromDb = from u in context.UserInfos
                                         select u;

                var SecurePassword = HashPassword(password); //Create new hash password

                Console.WriteLine("Enter username");
                userName = Console.ReadLine();
                Console.WriteLine("Enter password");
                password = Console.ReadLine();

                var newUser = new UserInfo(); //Add to database userinfo
                {
                    newUser.Username = userName;
                    newUser.HashedPassword = SecurePassword;
                }
                context.UserInfos.Add(newUser);
                context.SaveChanges();
                Console.WriteLine("userInfo added");

                foreach (var item in existingHashFromDb)
                {
                    Console.WriteLine("Hash from DB" + item.HashedPassword);
                }
            }
        }
        public void SignIn() //BCrypt
        {
            using (var context = new SchoolContext())
            {
                var password = "";
                var username = "";
                var hashedPasswordFromDB = "";
                var tempUsername = "";

                var GetHashedPassword = from u in context.UserInfos
                                        where u.Username == username
                                        select u;

                Console.Write("Enter username: ");
                username = Console.ReadLine();
                Console.Write("Enter password: ");
                password = Console.ReadLine();

                foreach (var item in GetHashedPassword)
                {
                    hashedPasswordFromDB = item.HashedPassword;
                    //tempUsername = item.Username;
                }
                var validateLogIn = ValidatePassword(password, hashedPasswordFromDB);
                Console.WriteLine(validateLogIn);
                //if (validateLogIn == true)
                //{
                //    Console.WriteLine("Logged in");
                //}
                //else
                //{
                //    Console.WriteLine("Not logged in");
                //}

                //Console.WriteLine(hashedPasswordFromDB);

            }

        }
    }
}
