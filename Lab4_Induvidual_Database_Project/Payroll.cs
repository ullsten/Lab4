using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Office2010.Drawing;
using Lab4_Induvidual_Database_Project;
using Lab4_Induvidual_Database_Project.Data;
using Lab4_Induvidual_Database_Project.Models;
using Labb4_Individual_Database_project;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using Menu = Labb4_Individual_Database_project.Menu;

namespace Lab4_Induvidual_Database_Project
{
    public class Payroll
    {
        public void PaymentMonthly() //OK
        {
            AnsiConsole.MarkupLine("[deeppink4_2]Getting your information[/]");
            Thread.Sleep(1000);
            AnsiConsole.MarkupLine("[magenta3]Oh no, the dog has shit on the floor..[/]");
            Thread.Sleep(1000);
            AnsiConsole.MarkupLine("[hotpink3]Done, here you have![/]");
            Console.WriteLine();
            Menu menu = new Menu();
            SqlConnection sqlcon = new SqlConnection(@"Data Source=ULLSTENLENOVO; Initial Catalog=School;Integrated Security=True");
            //Query bestämmer vilken data vill vi ha
            SqlDataAdapter sqlDat = new SqlDataAdapter("" +
                "SELECT PositionName AS Position, SUM (Salary) AS [Monthly payment] FROM StaffAdmin\r\n" +
                "Join Position ON FK_PositionId = PositionId\r\n" +
                "Group BY PositionName\r\n" +
                "order by [Monthly payment] DESC", sqlcon);
            //Tomt data table
            DataTable dtTbl = new DataTable();
            //Fyller data table
            sqlDat.Fill(dtTbl);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(new string('-', 36));
            Console.ResetColor();
            AnsiConsole.MarkupLine("[orange1]|[/] [green]{0, -14}[/] [orange1]|[/] [green]{1, -15}[/] [orange1]|[/]", "Department", "Monthly payment");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(new string('-', 36));
            foreach (DataRow dr in dtTbl.Rows)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                AnsiConsole.MarkupLine("[orange1]|[/] [green]{0, -14}[/] [orange1]|[/] [red]{1, -15}[/] [orange1]|[/]", 
                    dr["Position"], ((decimal)dr["Monthly payment"]).ToString("C2"));
                Console.ResetColor();
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(new string('-', 36));
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Enter to see menu again");
            Console.ResetColor();
            Console.ReadLine();
            menu.PayRollOffice();
        }
        public void PaymentYearly() //OK
        {
            AnsiConsole.MarkupLine("[palegreen1_1]Getting your information[/]");
            Thread.Sleep(1000);
            AnsiConsole.MarkupLine("[darkolivegreen3_2]Wait, just going to the bathroom[/]");
            Thread.Sleep(1000);
            AnsiConsole.MarkupLine("[greenyellow]Done, here you have![/]");
            Console.WriteLine();
            Menu menu = new Menu();
            SqlConnection sqlcon = new SqlConnection(@"Data Source=ULLSTENLENOVO; Initial Catalog=School;Integrated Security=True");
            //Query bestämmer vilken data vill vi ha
            SqlDataAdapter sqlDat = new SqlDataAdapter("SELECT PositionName AS Position, SUM(Salary*12) AS [Yearly salary] FROM StaffAdmin\r\n" +
                "Join Position ON FK_PositionId = PositionId\r\n" +
                "Group BY PositionName\r\n" +
                "Order BY [Yearly salary] DESC", sqlcon);
            //Tomt data table
            DataTable dtTbl = new DataTable();
            //Fyller data table
            sqlDat.Fill(dtTbl);

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            AnsiConsole.MarkupLine(new string('-', 39));
            Console.ResetColor();
            AnsiConsole.MarkupLine("[orange1]|[/] [green]{0, -14}[/] [orange1]|[/] [green]{1, -18}[/] [orange1]|[/]", "Department", "Annual cost salary");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(new string('-', 39));
            foreach (DataRow dr in dtTbl.Rows)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                AnsiConsole.MarkupLine("[orange1]|[/] [green]{0, -14}[/] [orange1]|[/] [red]{1, -18}[/] [orange1]|[/]", dr["Position"], ((decimal)dr["Yearly salary"]).ToString("C"));
                Console.ResetColor();
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(new string('-', 39));
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Enter to see menu again");
            Console.ResetColor();
            Console.ReadLine();
            menu.PayRollOffice();
        }
        public void AverageSalaryDepartment() //OK
        {
            AnsiConsole.MarkupLine("[tan]Getting your information[/]");
            Thread.Sleep(1000);
            AnsiConsole.MarkupLine("[lightgoldenrod3]Just a moment, need to take the phone[/]");
            Thread.Sleep(1000);
            AnsiConsole.MarkupLine("[gold3_1]Done, here you have![/]");
            Console.WriteLine();
            Menu menu = new Menu();
            SqlConnection sqlcon = new SqlConnection(@"Data Source=ULLSTENLENOVO; Initial Catalog=School;Integrated Security=True");
            //Query bestämmer vilken data vill vi ha
            SqlDataAdapter sqlDat = new SqlDataAdapter("" +
                "SELECT PositionName AS Position, AVG(Salary) AS [Average salary] FROM StaffAdmin\r\n" +
                "Join Position ON FK_PositionId = PositionId\r\n" +
                "Group BY PositionName\r\n" +
                "Order BY [Average salary] DESC", sqlcon);
            //Tomt data table
            DataTable dtTbl = new DataTable();
            //Fyller data table
            sqlDat.Fill(dtTbl);
           
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            AnsiConsole.MarkupLine(new string('-', 35));
            Console.ResetColor();
            AnsiConsole.MarkupLine("[orange1]|[/] [green]{0, -14}[/] [orange1]|[/] [green]{1, -14}[/] [orange1]|[/]", "Department", "Average salary");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(new string('-', 35));
            foreach (DataRow dr in dtTbl.Rows)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                AnsiConsole.MarkupLine("[orange1]|[/] [green]{0, -14}[/] [orange1]|[/] [red]{1, -14}[/] [orange1]|[/]", dr["Position"], ((decimal)dr["Average salary"]).ToString("C"));
                Console.ResetColor();
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(new string('-', 35));
            Console.ResetColor();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Enter to see menu again");
            Console.ResetColor();
            Console.ReadLine();
            menu.PayRollOffice();
        }
        public void SalaryIncrease() //Coming
        {
            //Employee employee = new Employee();
            Menu menu = new Menu();
            var staffName = "";
            decimal currentSalary = 0;
            var negotiate = "";
            var feelsOk = "";
            var salaryProposal2 = "";
            var youThink = "";
            decimal newSalary = 0;
            int staffId = 0;
            int desiredSalaryIncrease = 0;
            using (var context = new SchoolContext())
            { 
                Random rnd = new Random(); //Create random rnd
                decimal salaryProposal = rnd.Next(350,2500); //Random values in ()
                decimal salaryProposalLower = rnd.Next(250, 1000);

                AnsiConsole.Markup("[green]Welcome, you want to negotiate the salary?[/] [blue](Y/N)[/] ");
                negotiate = Console.ReadLine();
                while (true)
                {
                    if (negotiate.ToLower() == "y")
                    {
                        GetSalaryByPosition(out staffName, out currentSalary, out staffId); //Get current salary after position input
                        Console.WriteLine();
                        AnsiConsole.MarkupLine($"[gold3]{staffName}[/] [green]today your salary is[/] [yellow]{currentSalary.ToString("C")}[/]");
                        Console.WriteLine();
                        AnsiConsole.Markup($"[green]According to the collective agreement, you can get a[/] [yellow]{salaryProposal.ToString("C")}[/] [green]salary increase. Feels ok?[/] [blue](Y/N)[/] ");
                        feelsOk = Console.ReadLine();
                        if (feelsOk.ToLower() == "y")
                        {
                            var updateSalary = context.StaffAdmins.First(sa => sa.StaffAdminId == staffId); //Update salary for selected staff
                            if (updateSalary != null)
                            {
                                newSalary += salaryProposal + currentSalary;
                                updateSalary.Salary = newSalary;
                            }
                            context.SaveChanges();
                            Console.WriteLine(new string('-', 55));
                            AnsiConsole.MarkupLine($"[green]From[/] [grey58]{DateTime.Now.ToString("yyyy/MM/dd")}[/] [green]Your new salary is:[/] [yellow]{newSalary.ToString("C")}[/]");
                            Console.WriteLine(new string('-', 55));
                            AnsiConsole.MarkupLine("[grey58]|enter to get to the menu|[/]");
                            Console.ReadLine();
                            break;
                        }
                        else if (feelsOk.ToLower() == "n")
                        {
                            AnsiConsole.Markup("[gold3]How much do you think?[/] ");
                            var ownProposal = int.TryParse(Console.ReadLine(), out desiredSalaryIncrease);
                            while (true)
                            {
                                if (desiredSalaryIncrease > 2500)
                                {
                                    AnsiConsole.Markup($"[rosybrown]Sorry, but that´s not possible! I can give you[/] [yellow]{salaryProposalLower.ToString("C")}[/] [blue](Y/N)[/] ");
                                    salaryProposal2 = Console.ReadLine();
                                }
                                if (salaryProposal2.ToLower() == "y")
                                {
                                    var updateSalary = context.StaffAdmins.First(sa => sa.StaffAdminId == staffId); //Update salary for selected staff
                                    if (updateSalary != null)
                                    {
                                        newSalary += desiredSalaryIncrease + currentSalary;
                                        updateSalary.Salary = newSalary;
                                    }
                                    context.SaveChanges();
                                    Console.WriteLine(new string('-', 55));
                                    AnsiConsole.MarkupLine($"[green]From[/] [grey58]{DateTime.Now.ToString("yyyy/MM/dd")}[/] [green]Your new salary is:[/] [yellow]{newSalary.ToString("C")}[/]");
                                    Console.WriteLine(new string('-', 55));
                                    AnsiConsole.MarkupLine("[grey58]|enter to get back to menu|[/]");
                                    Console.ReadLine();
                                    menu.PayRollOffice();
                                    break;
                                }
                                else if (salaryProposal2.ToLower() == "n")
                                {
                                    AnsiConsole.MarkupLine("[blue]Ok, then you can come back another time![/]");
                                    Console.WriteLine();
                                    AnsiConsole.MarkupLine("[grey58]|enter to get to the menu|[/]");
                                    Console.ReadLine();
                                    menu.PayRollOffice();
                                    break;
                                }
                                if (desiredSalaryIncrease <= 2500)
                                {
                                    Console.WriteLine();
                                    AnsiConsole.MarkupLine("[gold3]Ok, I can agree with that![/]");
                                    var updateSalary = context.StaffAdmins.First(sa => sa.StaffAdminId == staffId); //Update salary for selected staff
                                    if (updateSalary != null)
                                    {
                                        newSalary += desiredSalaryIncrease + currentSalary;
                                        updateSalary.Salary = newSalary;
                                    }
                                    context.SaveChanges();
                                    Console.WriteLine(new string('-', 55));
                                    AnsiConsole.MarkupLine($"[green]From[/] [grey58]{DateTime.Now.ToString("yyyy/MM/dd")}[/] [green]Your new salary is:[/] [yellow]{newSalary.ToString("C")}[/]");
                                    Console.WriteLine(new string('-', 55));
                                    AnsiConsole.MarkupLine("[grey58]|enter to get to the menu|[/]");
                                    Console.ReadLine();
                                    menu.PayRollOffice();
                                    break;
                                }
                            }
                        }
                    }
                    else if (negotiate.ToLower()  == "n")
                    {
                        Console.WriteLine();
                        AnsiConsole.MarkupLine("[darkred_1]Ok, you can always come back another time![/]");
                        Console.WriteLine();
                        AnsiConsole.MarkupLine("[grey58]|enter for menu again|[/]");
                        Console.ReadLine();
                        menu.PayRollOffice();
                    }
                    else
                    {
                        AnsiConsole.Markup("[red]What did you say?[/] [blue](Y/N?[/] ");
                        negotiate = Console.ReadLine();
                    }
                }
            }
        }
        public void GetSalaryByPosition(out string staffName, out decimal currentSalary, out int staffId) //Get current salary by staffID
        {
            Employee employee = new Employee();
            staffName = ""; //stores current staff name for selected staff
            currentSalary = 0; //stores current salary for selected staff ID
            int selectedStaffId = 0;
            Console.WriteLine();
            ShowEmployees(); //Show employees with id to chose from
            Console.WriteLine();
            using (var context = new SchoolContext())
            {
                var totalStaff = from s in context.staff select s; //to get total staff to while loop
                AnsiConsole.Markup($"[green]Enter your employment ID:[/] ");
                while (true)
                {
                    if (int.TryParse(Console.ReadLine(), out selectedStaffId) && selectedStaffId >= 1 && selectedStaffId <= totalStaff.Count())
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"Select employee ID 1-{totalStaff.Count()}!");
                    }
                }
                staffId = selectedStaffId; //Get selected staff Id to use later
                // Get current salary from staffId
                var getSalaryById = from sa in context.StaffAdmins
                                    join s in context.staff on sa.FkStaffId equals s.StaffId
                                    join p in context.Positions on sa.FkStaffId equals p.PositionId
                                    where sa.FkStaffId == selectedStaffId
                                    select new
                                    {
                                        name = s.FirstName + " " + s.LastName,
                                        position = p.PositionName,
                                        currentS = sa.Salary
                                    };
                foreach (var item in getSalaryById)
                {
                    staffName = item.name;
                    currentSalary = (decimal)item.currentS;
                }
            }
        }
        public void ShowEmployees() //Internal method to show employees in get current salary by ID
        {
            using(var context = new SchoolContext())
            {
                var getEmployees = from s in context.staff select s;
                Console.WriteLine(new string('-', 26));
                AnsiConsole.MarkupLine("| [green]{0, -2}[/] | [green]{1, -17}[/] |", "Id", "Employee names");
                Console.WriteLine(new string('-', 26));
                foreach (var s in getEmployees)
                {
                    AnsiConsole.MarkupLine("| [yellow]{0, -2}[/] | [grey46]{1, -17}[/] | ", s.StaffId, s.FirstName + " " + s.LastName);
                }
                Console.WriteLine(new string('-', 26));
            }
        }
    }
}
