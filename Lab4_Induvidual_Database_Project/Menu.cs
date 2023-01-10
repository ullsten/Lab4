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
            "Personell list", "Enter new employee", "Count employees by position", 
            "Enter new student", "See students in a class", "See all registered students",
            "Show active/not active courses", "Get important information about the student by ID(SP)",
            "Enter grade for student(transaction)", "Other employee", 
            "Back to school start", "Go home"
            }));
            switch (adminChoices)
            {
                case "Personell list":          //OK
                    employee.GetAllStaff();
                    break;
                case "Enter new employee":                //OK
                    employee.SaveStaff();
                    break;
                case "Count employees by position": //OK
                    employee.CountPositions();
                    break;
                case "Enter new student":        //OK
                    pupil.EnrollmentStudent();
                    break;
                case "See students in a class":       //OK
                    pupil.ShowStudentInClass();
                    break;
                case "See all registered students":  //OK
                    pupil.ShowStudentInfo();
                    break;
                case "Show active/not active courses": //OK
                    pupil.ShowCoursesActiveNotActive();
                    break;
                case "Get important information about the student by ID(SP)": //OK
                    pupil.StoredProcedurId();
                    break;
                case "Enter grade for student(transaction)": //OK
                    pupil.SetGradeTransaction();
                    break;
                case "Other employee": //Not finished
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
            Employee employee = new Employee();
            School school = new School();
            var adminChoices = AnsiConsole.Prompt(
             new SelectionPrompt<string>()
             .Title("[green]What can we do for you today?[/]")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more choices)[/]")
            .AddChoices(new[] {
            "See my classmates", "Take new course", "Get my information(extra)", "See responsible teacher course", "Back to school start", "Go home"
            }));
            switch (adminChoices)
            {
                case "See my classmates":
                    pupil.SeeMyClassMates(); //ändra så att min klass visas
                    break;
                case "Take new course":
                    pupil.TakeNewCourse();
                    break;
                case "Get my information(extra)": //OK
                    pupil.ShowStudentExtraInfo();
                    break;
                case "Back to school start":
                    school.ShowSchoolStart();
                    break;
                case "See responsible teacher course":
                    employee.ResponsibleForCourse();
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
    

