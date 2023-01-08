using System;
using DocumentFormat.OpenXml.Office.CoverPageProps;
using Lab4_Induvidual_Database_Project;
using Lab4_Induvidual_Database_Project.Data;
using Microsoft.VisualBasic;
using Spectre.Console;


namespace Labb4_Individual_Database_project
{
    public class Program
    {
        static void Main(string[] args)
        {
            Payroll payroll = new Payroll();
            Validate validate = new Validate();
            Pupil pupil = new Pupil();
            School school = new School();
            Employee employee = new Employee();
            Admin admin = new Admin();
            Menu menu = new Menu();
            //school.ShowSchoolStart();

            
            var startDateCourse = DateTime.Now;
            Console.WriteLine(startDateCourse.ToString("yyyy/MM/dd"));

            //using (var context = new SchoolContext())
            //{
            //    //Show courses student can choose to take
            //    var getCourseToChoose = from e in context.Exams
            //                            join c in context.Courses on e.FkCourseId equals c.CourseId
            //                            where e.FkStudentId == 1
            //                            select c;
            //    foreach (var c in getCourseToChoose)
            //    {
            //        Console.WriteLine($"{c.CourseId} {c.CourseName}");
            //    }
            //}
        }
    }
}   
