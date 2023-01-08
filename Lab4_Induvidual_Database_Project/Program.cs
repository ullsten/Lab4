using System;
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
            pupil.ShowStudentExtraInfo();
            //pupil.SetGradeTransaction();
            //pupil.ShowStudentsNoGrade();

            //using (var context = new SchoolContext())
            //{
            //    var date1 = new DateTime(1982, 03, 04, 00, 00, 00);
            //    var teacherName = "";
            //    int teacherid = 0;
            //    var getdatofgrade = from e in context.Exams
            //                        join s in context.staff on e.FkStaffAdminId equals s.StaffId
            //                        where e.FkCourseId == 3
            //                        where e.FkStudentId == 2
            //                        select new
            //                        {
            //                            date = e.DateOfGrade,
            //                            name = s.FirstName + " " + s.LastName,
            //                            id = s.StaffId
            //                        };

            //    foreach (var item in getdatofgrade)
            //    {
            //        //Console.WriteLine(item.date);
            //        date1 = (DateTime)item.date;
            //        teacherName = item.name;
            //        teacherid = item.id;

            //    }

            //    Console.WriteLine($"nu funkar det fint! {date1} {teacherName} {teacherid}");
            //}
        }
    }
}   
