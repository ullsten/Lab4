using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab4_Induvidual_Database_Project;
using Lab4_Induvidual_Database_Project.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace Labb4_Individual_Database_project
{
    public class Employee
    {
       public void ShowEmployee()
        {
            using (var context = new SchoolContext())
            {
                var showEmp = from e in context.staff
                              select e;

                foreach (var e in showEmp)
                {
                    Console.WriteLine($"{e.StaffId} {e.FirstName} {e.LastName} {e.DayOfBirth} {e.Age} {e.SecurityNumber} {e.Gender} {e.HireDate} {e.YearOnSchool}");
                }
            }
        }
        public void GetAllStaff()
        {
            
            Menu menu = new Menu(); 
            SqlConnection sqlcon = new SqlConnection(@"Data Source=ULLSTENLENOVO; Initial Catalog=School;Integrated Security=True");
            //Query bestämmer vilken data vill vi ha
            SqlDataAdapter sqlDat = new SqlDataAdapter("SELECT * FROM [GetAllStaff]", sqlcon);
            //Tomt data table
            DataTable dtTbl = new DataTable();
            //Fyller data table
            sqlDat.Fill(dtTbl);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(new string('-', 87));
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("| {0, -2} | {1, -23} | {2, -18} | {3, -15} | {4, -13} |", "ID", "Name", "Date of employment", "Employed year", "Position");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(new string('-', 87));
            foreach (DataRow dr in dtTbl.Rows)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("| {0, -2} | {1, -23} | {2, -18} | {3, -15} | {4, -13} |", dr["StaffId"], dr["Name"], dr["Date of employment"], dr["Employed year"], dr["Position"]);
                Console.ResetColor();
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(new string('-', 87));
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Enter to see menu again");
            Console.ResetColor();
            Console.ReadLine();
            menu.AdminMenu();
        }
        public void SaveStaff()
        {
            Console.Write("Enter firstname: ");
            var firstname = Console.ReadLine();
            Console.Write("Enter lastname: ");
            var lastname = Console.ReadLine();
            Console.Write("Enter Security number: ");
            var ssn = Console.ReadLine();
            Console.Write("Street address: ");
            var street = Console.ReadLine();
            Console.Write("Postal code: ");
            var postalcode = Console.ReadLine();
            Console.Write("City: ");
            var city = Console.ReadLine();
            Console.Write("Homeland: ");
            var homeland = Console.ReadLine();
            //Show positions to choose from
            ShowPositions();
            int newPositionId;
            Console.Write("What position will you be empoloyed in?: ");
            while (true)
            {
                var positionId = Console.ReadLine();
                if (int.TryParse(positionId, out newPositionId) && newPositionId >= 1 && newPositionId <= 11)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Select position 1-11!");
                }
            }
            Console.WriteLine();
            try
            {
                string conString = "Data Source=ULLSTENLENOVO; Initial Catalog=School;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    //SQL-code
                    SqlCommand cmd = new SqlCommand("INSERT INTO Staff (FirstName, LastName, SecurityNumber, HireDate) VALUES(@firstname, @lastname ,@ssn, GETDATE())\r\n" +
                "INSERT INTO Address (StreetAddress, PostalCode, City, Homeland) VALUES (@street, @postalcode, @city, @homeland)\r\n" +
                "INSERT INTO StaffAdmin (FK_StaffId, FK_PositionId, FK_AddressId) VALUES (IDENT_CURRENT('Staff'), @positionId, IDENT_CURRENT('Address'))\r\n" +
                "UPDATE Staff SET DayOfBirth = SUBSTRING(SecurityNumber, 3,6) FROM Staff\r\n" +
                "UPDATE Staff SET Age = DATEDIFF(year,DayOfBirth,GETDATE()) Where StaffId = IDENT_CURRENT('Staff')\r\n" +
                "UPDATE Staff SET YearOnSchool = DATEDIFF(year,HireDate,GETDATE())\r\n" +
                "UPDATE Staff\r\nSET Gender = (CASE WHEN right(rtrim(SecurityNumber),1) IN ('1', '3', '5', '7', '9') THEN 'Male'\r\n" +
                "WHEN right(rtrim(SecurityNumber), 1) IN ('2', '4', '6', '8', '0') THEN 'Female' END)\r\n" +
                "Where StaffId = IDENT_CURRENT('Staff')", connection);
                    //open connection to base
                    connection.Open();
                    //set input value to sql-query
                    cmd.Parameters.AddWithValue("@firstname", firstname);
                    cmd.Parameters.AddWithValue("@lastname", lastname);
                    cmd.Parameters.AddWithValue("@ssn", ssn);
                    cmd.Parameters.AddWithValue("@street", street);
                    cmd.Parameters.AddWithValue("@postalcode", postalcode);
                    cmd.Parameters.AddWithValue("@city", city);
                    cmd.Parameters.AddWithValue("@homeland", homeland);
                    cmd.Parameters.AddWithValue("@positionId", newPositionId);
                    //Reads rows from table in data base
                    SqlDataReader sdr = cmd.ExecuteReader();
                    
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine();
                    Console.WriteLine($"We welcome {firstname} {lastname}");
                    Console.WriteLine($"Hope you enjoy our school!");
                    Console.ResetColor();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("OOPs, Somthing went wrong" + e);
            }
            Console.WriteLine();
            Console.WriteLine();
            moreToAdd();
        }
        public void CountPositions()
        {
            using (var context = new SchoolContext())
            {
                ShowPositions();//Show positions to choose from
                Console.WriteLine();
                Console.WriteLine("Select a position to see how many peaople work there [1-11]");
                int selectedId = 0;
                while (true) //Check to for correct input format (int)
                {
                    if (int.TryParse(Console.ReadLine(), out selectedId) && selectedId >= 1 && selectedId <= 11)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Select position 1-11!");
                    }
                }
                var name = ""; //stores position name to use in output
                //EF code to get positionName for use in print
                var countPosition = from cp in context.StaffAdmins
                                     join p in context.Positions on cp.FkPositionId equals p.PositionId
                                     where p.PositionId == selectedId
                                     select p;
                foreach (var p in countPosition) //loop to get position name
                {
                    name = p.PositionName;
                }
                AnsiConsole.MarkupLine($"[green]Number of[/] [yellow]{name}[/] [green]in the school is:[/] [blue]{countPosition.Count()}[/]");
                Console.WriteLine();
                MoreCountPosition();
            }
        }

        private void moreToAdd()
        {
            Menu menu = new Menu();
            Console.WriteLine("Are there more employees to be registered today? [Y/N]");
            var moreAdd = Console.ReadLine();
            if (moreAdd.ToLower() == "y")
            {
                SaveStaff();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("You find out the way out yourself");
                Console.ResetColor();
                menu.AdminMenu();
            }
        }
        public void ShowPositions()
        {
            using (var context = new SchoolContext())
            {
                var getPostions = from p in context.Positions
                                  orderby p.PositionId
                                  select p;

                Console.WriteLine(new string('-', 27));
                Console.WriteLine("| {0, -2} | {1, -18} |", "ID", "Position");
                Console.WriteLine(new string('-', 27));
                foreach (var p in getPostions)
                {
                    Console.WriteLine("| {0, -2} | {1, -18} |", p.PositionId, p.PositionName);
                }
                Console.WriteLine(new string('-', 27));

            }
        }
        private void MoreCountPosition()
        {
            Menu menu = new Menu();
            if (!AnsiConsole.Confirm("Do you want to count another position?")) //Prompt for new serch or not
            {
                menu.AdminMenu();
            }
            else
            {
                Console.Clear();
                CountPositions();
            }
        }
    }
}

