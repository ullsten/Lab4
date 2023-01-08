using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.Drawing;
using Lab4_Induvidual_Database_Project;
using Lab4_Induvidual_Database_Project.Data;
using Lab4_Induvidual_Database_Project.Models;
using Labb4_Individual_Database_project;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

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
                AnsiConsole.MarkupLine("[orange1]|[/] [green]{0, -14}[/] [orange1]|[/] [red]{1, -15}[/] [orange1]|[/]", dr["Position"], ((decimal)dr["Monthly payment"]).ToString("C"));
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
        public async void AverageSalaryDepartment() //OK
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
    }
}
