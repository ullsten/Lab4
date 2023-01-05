using Lab4_Induvidual_Database_Project;
using Spectre.Console;

namespace Labb4_Individual_Database_project
{
    public class Program
    {
        static void Main(string[] args)
        {
            Payroll payroll = new Payroll();
            Validate validate = new Validate();
            Pupil pupil= new Pupil();
            School school = new School();
            Employee employee= new Employee();
            Admin admin = new Admin();
            Menu menu= new Menu();
            //school.ShowSchoolStart();

            //pupil.StoredProcedurId();
            pupil.SetGradeTransaction();
            
        }
    }
}