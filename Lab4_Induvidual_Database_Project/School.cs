using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;
using Gma.System.MouseKeyHook;
using Lab4_Induvidual_Database_Project.Data;
using Lab4_Induvidual_Database_Project.Models;
using Spectre.Console;

namespace Labb4_Individual_Database_project
{

    public class School
    {
        public void ShowSchoolStart()
        {
            Console.Clear();
            Menu menu = new Menu();
       
            using (var context = new SchoolContext()) 
            {
                var grid = new Grid();
                // Add columns 
                grid.AddColumn();
                grid.AddColumn();
                grid.AddColumn();
                // Add header row 
                grid.AddRow(new string[] { "Header 1", "Header 2", "Header 3" });
                grid.AddRow(new string[] { "Row 1", "Row 2", "Row 3" });

                //Manage panel courses inside table
                var courses = new Panel(
                    "- Pudding pan with eggs & parsley\n" +
                    "- Pasta gratin with paprika pesto\n" +
                    "- Salmon with cauliflower and dill cream\n" +
                    "- Chicken wok with teriyaki and lime\n" +
                    "- Chickpea stew with rice\n" +
                    "\n" +
                    "\n" +
                    "All ingredientes are carefully selected adn with local roots");
                courses.Header = new PanelHeader("Food voucher w.51");
                courses.Expand();
                courses.BorderColor(Color.Green3);
                courses.HeaderAlignment(Justify.Center);
                //Manage panel schedule inside table
                var schedule = new Panel(
                 "08:00-09:00 Math\n" +
                 "09:30-11:00 Gymnastics\n" +
                 "11:00-12:00 Lunch\n" +
                 "12:00-13:00 Science\n" +
                 "13:15-14:30 History\n" +
                 "14:45-16:00 Economy\n");

                schedule.Header = new PanelHeader("Schedule");
                schedule.Expand();
                schedule.BorderColor(Color.Green3);
                schedule.HeaderAlignment(Justify.Center);
                //manage calender in table
                var calendar = new Calendar(2023, 01);
                calendar.AddCalendarEvent(2023, 12, 24);
                calendar.AddCalendarEvent(2023, 12, 31);
                calendar.HighlightStyle(Style.Parse("yellow bold"));
                calendar.HeaderStyle(Style.Parse("blue bold"));
                // Create a table
                var table = new Table();
            table.Title("SCHOOL INTRANET");
            table.Expand();
            table.BorderColor(Color.Grey46);
            table.Centered();
            table.Width = 150;
            table.DoubleEdgeBorder();
            // Add some columns
            table.AddColumn((new TableColumn(schedule)));
            table.AddColumn(new TableColumn(courses)).LeftAligned();
            table.AddColumn(new TableColumn(calendar));
            table.Columns[0].Width(5);
            table.Columns[1].Width(15);
            table.Columns[2].Width(15);
            // Add some rows
            
            //table.AddRow("1", "jj", "jj");
            //table.AddRow(new Markup("[blue]Corgi[/]"));
           // table.AddRow(new Markup("[blue]Corgi[/]"), new Panel("Waldo"));
            //table.AddRow(new Markup("[blue]Corgi[/]"), new Panel("Waldo"));
            // Render the table to the console
            AnsiConsole.Write(table);

            }
            AnsiConsole.WriteLine();
            var department = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
             .Title("[green]Select your department for more choices.[/]")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
            .AddChoices(new[] {
            "Administration", "Student", "Payroll office", "Go home"}));
            switch (department)
            {
                case "Administration":
                    menu.AdminMenu();
                    break;
                case "Student":
                    menu.PupilMenu();
                    break;
                case "Payroll office":
                    menu.PayRollOffice();
                    break;
                case "Go home":
                    Console.ForegroundColor= ConsoleColor.Magenta;
                    Console.WriteLine("Did you go wrong?");
                    Console.ResetColor();
                    Thread.Sleep(1500);
                    Environment.Exit(0); 
                    break;
            }
            Console.ReadLine();
        }
    }
}

