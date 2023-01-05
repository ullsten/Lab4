using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Lab4_Induvidual_Database_Project;
using Lab4_Induvidual_Database_Project.Data;
using Lab4_Induvidual_Database_Project.Models;
using Microsoft.EntityFrameworkCore;
using Spectre.Console;

namespace Labb4_Individual_Database_project
{
    public class Menu
    {
        public void EmployeeMenu()
        {
            Employee employee = new Employee();
            employee.ShowPositions();
            Console.WriteLine("Choose your location for more options.");
            var location = Console.ReadLine();
            switch (location)
            {
                case "":

                default:
                    break;
            }
            Console.ReadLine();
        }
        public void AdminMenu()
        {
            Console.Clear();
            Admin admin = new Admin();
            Payroll payroll = new Payroll();
            School school = new School();
            Pupil pupil = new Pupil();
            Employee employee = new Employee();
            var adminChoices = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
             .Title("[green]What can we do for you today?[/]")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
            .AddChoices(new[] {
            "Personell list", "Save Staff", "Count employee position", 
            "Save new student", "Students by class", "Students in school",
            "Show active/not active courses", "Student information by Id",
            "Set grade", "Other employee", 
            "Back to school start", "Go home"
            }));
            switch (adminChoices)
            {
                case "Personell list":          //OK
                    employee.GetAllStaff();
                    break;
                case "Save staff":                //OK
                    employee.SaveStaff();
                    break;
                case "Count employee position": //OK
                    employee.CountPositions();
                    break;
                case "Save new student":        //OK
                    pupil.EnrollmentStudent();
                    break;
                case "Students by class":       //OK
                    pupil.ShowStudentInClass();
                    break;
                case "Students in school":  //OK
                    pupil.ShowStudentInfo();
                    break;
                case "Show active/not active courses":
                    pupil.ShowCoursesActiveNotActive();
                    break;
                case "Student information by Id":
                    pupil.StoredProcedurId();
                    break;
                case "Set grade(transaction)":
                    pupil.SetGradeTransaction();
                    break;
                case "Other employee":
                    EmployeeMenu();
                    break;
                case "Back to school start":
                    school.ShowSchoolStart();
                    break;
                case "Go home":
                    Console.WriteLine("Have a nice day!");
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
            Console.ReadLine();
        }
        public void PupilMenu()
        {
            Console.Clear();
            Pupil pupil = new Pupil();
            School school = new School();
            var adminChoices = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
             .Title("[green]What can we do for you today?[/]")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
            .AddChoices(new[] {
            "Students in my class", "See my courses", "Back to school start", "Go home"
            }));
            switch (adminChoices)
            {
                case "Students in my class":
                    pupil.ShowStudentInClass(); //OK
                    break;
                case "Take new course":      
                    Console.WriteLine("Take new course here! coming!");
                    break;
                case "Back to school start":
                    school.ShowSchoolStart();
                    break;
                case "Go home":
                    Console.WriteLine("Have a nice day!");
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
        public void PayRollOffice()
        {
            Console.Clear();
            Payroll payroll = new Payroll();
            School school = new School();
             var payRolChoices = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
             .Title("[green]Select your department for more choices.[/]")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
            .AddChoices(new[] {
            "Payment monthly", "Payment yearly", "Average salary",
            "Back to school start", "Go home"
            }));
            switch (payRolChoices)
            {
                case "Payment monthly":
                    payroll.PaymentMonthly();
                    break;
                case "Payment yearly":
                    payroll.PaymentYearly();
                    break;
                case "Average salary":
                    payroll.AverageSalaryDepartment();
                    break;
                case "Back to school start":
                    school.ShowSchoolStart();
                    break;
                case "Go home":
                    Console.ForegroundColor= ConsoleColor.Magenta;
                    Console.WriteLine("Hope you are satisfied with the help, otherwise you can come back.");
                    Console.ResetColor();
                    Thread.Sleep(1000);
                    Environment.Exit(0);
                    break;
            }
        }
    }
}
    

