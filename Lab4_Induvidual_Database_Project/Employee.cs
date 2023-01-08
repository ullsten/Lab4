using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Wordprocessing;
using Lab4_Induvidual_Database_Project;
using Lab4_Induvidual_Database_Project.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace Labb4_Individual_Database_project
{
    public class Employee
    {

        public void GetAllStaff() //OK
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
            Console.WriteLine(new string('-', 90));
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("| {0, -2} | {1, -23} | {2, -18} | {3, -15} | {4, -15} |", "ID", "Name", "Date of employment", "Employed year", "Position");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(new string('-', 90));
            foreach (DataRow dr in dtTbl.Rows)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("| {0, -2} | {1, -23} | {2, -18} | {3, -15} | {4, -15} |", dr["StaffId"], dr["Name"], dr["Date of employment"], dr["Employed year"], dr["Position"]);
                Console.ResetColor();
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(new string('-', 90));
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
            AnsiConsole.Markup("[navajowhite3]Enter firstname:[/] ");
            var firstname = Console.ReadLine();
            AnsiConsole.Markup("[navajowhite3]Enter lastname:[/] ");
            var lastname = Console.ReadLine();
            AnsiConsole.Markup("[navajowhite3]Enter Security number:[/] ");
            var ssn = Console.ReadLine();
            AnsiConsole.Markup("[lightsteelblue3]Street address:[/] ");
            var street = Console.ReadLine();
            AnsiConsole.Markup("[lightsteelblue3]Postal code:[/] ");
            var postalcode = Console.ReadLine();
            AnsiConsole.Markup("[lightsteelblue3]City:[/] ");
            var city = Console.ReadLine();
            AnsiConsole.Markup("[lightsteelblue3]Homeland:[/] ");
            var homeland = Console.ReadLine();
            //Show positions to choose from
            ShowPositions();
            int newPositionId;
            AnsiConsole.Markup("[darkgoldenrod]What position will you be empoloyed in?:[/] ");
            while (true)
            {
                var positionId = Console.ReadLine();
                if (int.TryParse(positionId, out newPositionId) && newPositionId >= 1 && newPositionId <= 11)
                {
                    break;
                }
                else
                {
                   AnsiConsole.MarkupLine("[red]Select position 1-11![/]");
                }
            }
            decimal salaryInput = 0;
            AnsiConsole.Markup("[gold3]What salary is agreed?[/]");
            while (true)
            {
                var setSalary = Console.ReadLine();
                if (decimal.TryParse(setSalary, out salaryInput))
                {
                    break;
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Now it was wrong, enter the amount in numbers![/]");
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
                "INSERT INTO StaffAdmin (FK_StaffId, FK_PositionId, FK_AddressId, Salary) VALUES (IDENT_CURRENT('Staff'), @positionId, IDENT_CURRENT('Address'), @salaryInput)\r\n" +
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
                    cmd.Parameters.AddWithValue("@salaryInput", salaryInput);

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
        } //OK
        public void CountPositions() //OK
        {
            using (var context = new SchoolContext())
            {
                ShowPositions();//Show positions to choose from
                Console.WriteLine();
                AnsiConsole.MarkupLine("[lightpink3]Select a position to see how many peaople work there[/] [cornsilk1](1-11)[/]");
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
                var positionName = ""; //stores position name to use in output
                //EF code to get positionName for use in print
                var countPosition = from s in context.StaffAdmins
                                    join p in context.Positions on s.FkPositionId equals p.PositionId
                                    where p.PositionId == selectedId
                                    select s;
                //get name for selected position id
                var getPositionName = from p in context.Positions
                                      where p.PositionId == selectedId
                                      select p;
                foreach (var p in getPositionName) { positionName = p.PositionName; }
                //Print out result
                AnsiConsole.MarkupLine($"[green]Number of[/] [yellow]{positionName}[/] [green]in the school is:[/] [blue]{countPosition.Count()}[/]");
                Console.WriteLine();
                MoreCountPosition();
            }
        }
        public void ResponsibleForCourse()
        {
            Menu menu = new Menu();
            SqlConnection sqlcon = new SqlConnection(@"Data Source=ULLSTENLENOVO; Initial Catalog=School;Integrated Security=True");
            //Query bestämmer vilken data vill vi ha
            SqlDataAdapter sqlDat = new SqlDataAdapter("SELECT * FROM [Responsible for course]", sqlcon);
            //Tomt data table
            DataTable dtTbl = new DataTable();
            //Fyller data table
            sqlDat.Fill(dtTbl);

            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(new string('-', 48));
            Console.ResetColor();
            AnsiConsole.MarkupLine("[navyblue]|[/] [grey74]{0, -18}[/] [navyblue]|[/] [grey74]{1, -23}[/] [navyblue]|[/]", "Teacher name", "Responsible for course");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(new string('-', 48));
            foreach (DataRow dr in dtTbl.Rows)
            {
                AnsiConsole.MarkupLine("[navyblue]|[/] [grey74]{0, -18}[/] [navyblue]|[/] [grey74]{1, -23}[/] [navyblue]|[/]", dr["Teacher name"], dr["Responsible for course"]);
            }
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine(new string('-', 48));
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Enter to see menu again");
            Console.ResetColor();
            Console.ReadLine();
            menu.PupilMenu();
        } //OK
        //*************************************************
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
        } //OK Internal method
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
        } //OK internal method
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
        } //OK internal method
        public void SaveStaffTransaction() //INcorrect syntax
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
            decimal salaryInput = 0;
            Console.WriteLine("What salary is agreed?");
            while (true)
            {
                var setSalary = Console.ReadLine();
                if (decimal.TryParse(setSalary, out salaryInput))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Select position 1-11!");
                }
            }
            int returnRows = 0;
            string conString = "Data Source=ULLSTENLENOVO; Initial Catalog=School;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(conString))
            {
                //string with query
                string SqlSaveStaffTransaction =
                    $"INSERT INTO Staff (FirstName, LastName, SecurityNumber, HireDate) VALUES({firstname}, {lastname} ,{ssn}, GETDATE())\r\n\t\t" +$"" +
                    $"INSERT INTO Address (StreetAddress, PostalCode, City, Homeland) VALUES ({street}, {postalcode}, {city}, {homeland})\r\n\t\t" +$"" +
                    $"INSERT INTO StaffAdmin (FK_StaffId, FK_PositionId, FK_AddressId, Salary) VALUES (IDENT_CURRENT('Staff'), {newPositionId}, IDENT_CURRENT('Address'), {salaryInput})\r\n\t\t" +"" +
                    "UPDATE Staff SET DayOfBirth = SUBSTRING(SecurityNumber, 3,6) FROM Staff\r\n\t\t" +"" +
                    "UPDATE Staff SET Age = DATEDIFF(year,DayOfBirth,GETDATE()) Where StaffId = IDENT_CURRENT('Staff')\r\n\t\t" +"" +
                    "UPDATE Staff SET YearOnSchool = DATEDIFF(year,HireDate,GETDATE())\r\n\t\t" +"" +
                    "UPDATE Staff SET Gender = (CASE WHEN right(rtrim(SecurityNumber),1) IN ('1', '3', '5', '7', '9') THEN 'Male'\r\n\t\tWHEN right(rtrim(SecurityNumber), 1) IN ('2', '4', '6', '8', '0') " +
                    "THEN 'Female' END)" +"Where StaffId = IDENT_CURRENT('Staff')";
                //Open connection do database
                connection.Open();
                // Start a local transaction.
                SqlTransaction sqlTran = connection.BeginTransaction();
                // Enlist a command in the current transaction.
                SqlCommand command = connection.CreateCommand();
                command.Transaction = sqlTran;
                try
                {
                    // Execute command with string query
                    command.CommandText = SqlSaveStaffTransaction;
                    //returns rows affected
                    returnRows = command.ExecuteNonQuery();
                    //Commit the transaction.
                    sqlTran.Commit();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(new string('-', 53));
                    Console.ResetColor();
                    //Print result if transactions went well.
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine();
                    Console.WriteLine($"We welcome {firstname} {lastname}");
                    Console.WriteLine($"Hope you enjoy our school!");
                    Console.ResetColor();
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    AnsiConsole.MarkupLine(new string('-', 53));
                    Console.ResetColor();
                    AnsiConsole.MarkupLine($"[grey30]{returnRows} row were written to database.[/]");
                }
                catch (Exception ex)
                {
                    // Handle the exception if the transaction fails to commit.
                    AnsiConsole.MarkupLine($"[red]Now something went wrong,[/] [gold1]{returnRows}[/] [red]row was updated![/]");
                    Console.WriteLine(ex);
                    try
                    {
                        // Attempt to roll back the transaction.
                        sqlTran.Rollback();
                    }
                    catch (Exception exRollback)
                    {
                        // Throws an InvalidOperationException if the connection
                        // is closed or the transaction has already been rolled
                        // back on the server.
                        Console.WriteLine(exRollback.Message);
                    }
                }
            }
        } 
    }
}

