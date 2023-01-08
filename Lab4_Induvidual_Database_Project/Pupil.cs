using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Wordprocessing;
//using DocumentFormat.OpenXml.Wordprocessing;
using Lab4_Induvidual_Database_Project;
using Lab4_Induvidual_Database_Project.Data;
using Lab4_Induvidual_Database_Project.Models;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Color = Spectre.Console.Color;
using Style = Spectre.Console.Style;

namespace Labb4_Individual_Database_project
{
    internal class Pupil
    {
        public void ShowStudentInfo() //OK
        {
            Console.Clear();
            Menu menu = new Menu();
            using (var context = new SchoolContext())
            {
                //Code to get data from database
                var showStudent = from s in context.Students
                                  join a in context.Addresses on s.FkAddressId equals a.AddressId
                                  join c in context.Classes on s.FkClassId equals c.ClassId
                                  orderby s.StudentId
                                  select new
                                  {
                                      s.StudentId,
                                      Name = s.FirstName + " " + s.LastName, //Concat first and lastname
                                      s.Age,
                                      s.SecurityNumber,
                                      s.Gender,
                                      a.StreetAddress,
                                      a.PostalCode,
                                      a.City,
                                      a.Homeland,
                                      c.ClassName
                                  };
                AnsiConsole.MarkupLine(new string('-', 122));
                AnsiConsole.MarkupLine("| [green]{0, -3}[/] | [green]{1, -20}[/] | [green]{2, -3}[/] | [green]{3, -15}[/] | [green]{4, -6}[/] | [green]{5, -20}[/] | [green]{6, -5}[/] | [green]{7, -12}[/] | [green]{8, -10}[/] |", 
                    "Id","Name", "Age", "Security number", "Gender", "Street name", "PIN", "City", "Homeland", "Class in school");
                AnsiConsole.MarkupLine(new string('-', 122));
                foreach (var s in showStudent) //Loop to get info to console
                {
                    AnsiConsole.MarkupLine("| [grey46]{0, -3}[/] | [yellow]{1, -20}[/] | [grey46]{2, -3}[/] | [yellow]{3, -15}[/] | [yellow]{4, -6}[/] | [yellow]{5, -20}[/] | [grey46]{6, -5}[/] | [yellow]{7, -12}[/] | [yellow]{8, -10}[/] |", 
                        s.StudentId, s.Name, s.Age, s.SecurityNumber, s.Gender, s.StreetAddress, s.PostalCode, s.City, s.Homeland, s.ClassName);
                }
                AnsiConsole.MarkupLine(new string('-', 122));
            }
            AnsiConsole.MarkupLine("[grey46]Enter for menu[/]");
            Console.ReadLine();
            menu.PupilMenu();
        }
        public void EnrollmentStudent()
        {
            int staffadminId = 0;
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

            ShowClasses();//Show classes to choose from
            int selectedClassId = 0;
            while (true) //Check for correct input and between wright value
            {
                Console.WriteLine("Select class for student: ");
                if (int.TryParse(Console.ReadLine(), out selectedClassId) && selectedClassId >= 1 && selectedClassId <= 4)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Select class 1-4!");
                }
            }
            ShowActiveCourses(); //Show courses to choose from, shows only active courses
            int selectedCourseId = 0;
            Console.WriteLine("Whitch course should the student take?: ");
            var courseInput = Console.ReadLine();
            while (true)
            {
                if (int.TryParse(courseInput, out selectedCourseId) && selectedCourseId >= 1 && selectedCourseId <= 6)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Select course 1-6!");
                }
            }
           
            switch (selectedCourseId) //Set right teacher to course
            {
                case 1:
                    staffadminId = 4;
                    break;
                case 2:
                    staffadminId = 5;
                    break;
                case 3:
                    staffadminId = 6;
                    break;
                case 4:
                    staffadminId = 7;
                    break;
                case 5:
                    staffadminId = 15;
                    break;
                case 6:
                    staffadminId = 20;
                    break;
            }
            Console.WriteLine();
            try
            {
                string conString = "Data Source=ULLSTENLENOVO; Initial Catalog=School;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    //SQL-code
                    SqlCommand cmd = new SqlCommand("INSERT INTO Student (FirstName, LastName, SecurityNumber, FK_AddressId, FK_ClassId)\r\n" +
                        "VALUES (@firstname, @lastname, @ssn, IDENT_Current('Address'), @classId)\r\n" +
                        "INSERT INTO Address (StreetAddress, PostalCode, City, Homeland)\r\n" +
                        "VALUES (@street, @postalcode, @city, @homeland)\r\n" +
                        "INSERT INTO Exam(StartDateCourse, FK_StudentId, FK_CourseId, FK_StaffAdminId)\r\n" +
                        "VALUES (DATEADD(day, -15, GETDATE()), IDENT_CURRENT('Student'), @courseId, @staffadminid)\r\n" +
                        "UPDATE Student SET DayOfBirth = SUBSTRING(SecurityNumber, 3,6) FROM Student\r\n" +
                        "UPDATE Student SET Age = DATEDIFF(year,DayOfBirth,GETDATE()) Where StudentId = IDENT_CURRENT('Student')\r\n" +
                        "UPDATE Student\r\nSET Gender = (CASE WHEN right(rtrim(SecurityNumber),1) IN ('1', '3', '5', '7', '9') THEN 'Male'\r\nWHEN right(rtrim(SecurityNumber), 1) IN ('2', '4', '6', '8', '0') THEN 'Female' END)\r\n" +
                        "Where StudentId = IDENT_CURRENT('Student')", connection);
                    //open connection to base
                    connection.Open();
                    //set input value to sql-query
                    cmd.Parameters.AddWithValue("@firstname", firstname);
                    cmd.Parameters.AddWithValue("@lastname", lastname);
                    cmd.Parameters.AddWithValue("@ssn", ssn);
                    cmd.Parameters.AddWithValue("@classId", selectedClassId);
                    cmd.Parameters.AddWithValue("@street", street);
                    cmd.Parameters.AddWithValue("@postalcode", postalcode);
                    cmd.Parameters.AddWithValue("@city", city);
                    cmd.Parameters.AddWithValue("@homeland", homeland);
                    cmd.Parameters.AddWithValue("@courseId", selectedCourseId);
                    //cmd.Parameters.AddWithValue("@randomGradeId", randomGradeId);
                    cmd.Parameters.AddWithValue("@staffadminid", staffadminId);
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
            SaveMoreStudent();
        } //OK
        public void ShowStudentInClass() //OK
        {
            ShowClasses(); //Get classes from database to show
            int selectedId = 0;
            AnsiConsole.MarkupLine("Whitch [blue]class[/] do you want to see students from? [blue](1-4)[/]");
            while (true)//Check thats input is INT and between 1-6
            {
                var positioninput = Console.ReadLine();
                if (int.TryParse(positioninput, out selectedId) && selectedId >= 1 && selectedId <= 4)
                {
                    try
                    {
                        string conString = "Data Source=ULLSTENLENOVO; Initial Catalog=School;Integrated Security=True";
                        using (SqlConnection connection = new SqlConnection(conString))
                        {
                            //SQL-code
                            SqlCommand cmd = new SqlCommand("" +
                                "SELECT ClassName, CONCAT(FirstName, ' ', LastName) AS Name FROM Student\r\n" +
                                "Join Class ON FK_ClassId = ClassId\r\n" +
                                "Where ClassId = @getPosition", connection);
                            //open connection to base
                            connection.Open();
                            cmd.Parameters.AddWithValue("@getPosition", selectedId); //set input value to sql-query
                            SqlDataReader sdr = cmd.ExecuteReader();
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine(new string('-', 30));
                            Console.ResetColor();
                            AnsiConsole.MarkupLine("[grey46]|[/] [royalblue1]{0, -5}[/] [grey46]|[/] [royalblue1]{1, -18}[/] [grey46]|[/]", "Class", "Name");
                            Console.ResetColor();
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine(new string('-', 30));
                            Console.ResetColor();
                            Console.ForegroundColor = ConsoleColor.Green;
                            while (sdr.Read())
                            {
                                AnsiConsole.MarkupLine("[grey46]|[/] [steelblue1_1]{0, -5}[/] [grey46]|[/] [steelblue1_1]{1, -18}[/] [grey46]|[/]", sdr["ClassName"], sdr["Name"]);
                            }
                            Console.WriteLine(new string('-', 30));
                            Console.ResetColor();
                            Console.WriteLine();
                            Console.WriteLine();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("OOPs, Somthing went wrong" + e);
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Select position 1-4!");
                    Console.ResetColor();
                    continue;
                }
                //Ask if user want to see other positions
                SeeOtherClass();
            }
        }
        
        public void SaveCradeCourses() //Ok
        {
            Menu menu = new Menu();
            
            using (var context = new SchoolContext()) 
            {
                int selectedStudentId = 0; //stores studentId
                int selectedCourseId = 0; //stores courseId
                var totalCoursesStudent = 0; //stores students courses to use as lenght
                var totalStudents = 0; //store totalStudents here
                var chosedStudentName = ""; //stores name of selected student for later use
                var choosedCourseName = ""; //stores name of selected course for later use
                var teacherName = "";
                int teacherId = 0;
                var dateOfGrade = new DateTime(1982, 03, 04, 00, 00, 00);

                totalStudents = context.Students.Count();  //Count total students in school to use as lenght when select id to show
                //Get students name for later use
                var getStudentName = from s in context.Students
                                     where s.StudentId == selectedStudentId
                                     select s;
                //Get course name for later use
                var getSelectedCourseName = from c in context.Courses
                                            where c.CourseId == selectedCourseId
                                            select c;
                //Get name of teacher and date of grade
                var getNameTeacher = from e in context.Exams
                                     join s in context.staff on e.FkStaffAdminId equals s.StaffId
                                     where e.FkCourseId == selectedCourseId
                                     where e.FkStudentId == selectedStudentId
                                     select new
                                     {
                                         dateOfGrade = e.DateOfGrade,
                                         teacherName = s.FirstName + " " + s.LastName,
                                         teacherId = s.StaffId
                                     };
                ShowStudentsNoGrade();
                Console.WriteLine();
                AnsiConsole.Markup("[dodgerblue1]Which student do you want to enter grades for?[/] [grey42](enter id)[/] ");

                while (true) //Check for correct input and between wright value
                {
                    if (int.TryParse(Console.ReadLine(), out selectedStudentId) && selectedStudentId >= 1 && selectedStudentId <= totalStudents)
                    {
                        break;
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Select student id![/]");
                    }
                }
                Console.WriteLine();
                ShowCoursesStudent(selectedStudentId, out totalCoursesStudent);//Show course by student id where no grade is assigned.
                Console.WriteLine();
                AnsiConsole.Markup("[deepskyblue1]Which course should be graded?[/][grey42](enter id)[/] ");


                while (true) //Check for correct input and between wright value
                {
                    if (int.TryParse(Console.ReadLine(), out selectedCourseId) && selectedCourseId >= 1 && selectedCourseId <= totalCoursesStudent)
                    {
                        break;
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Select course id![/]");
                    }
                }
                Console.WriteLine();
                AnsiConsole.Markup("[deepskyblue2]What grade has the student received?[/] [grey46](1-5)[/] ");
                int selectedGradeId = 0;
                while (true) //Check for correct input and between wright value
                {
                    if (int.TryParse(Console.ReadLine(), out selectedGradeId) && selectedGradeId >= 1 && selectedGradeId <= 5)
                    {
                        break;
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]Select grade[/] [yellow]1-5[/]");
                    }
                }
                foreach (var item in getStudentName) //get student name to use in print out
                {
                    chosedStudentName = item.FirstName + " " + item.LastName;
                }
                foreach (var item in getSelectedCourseName)//Get cours name to use in print out
                {
                    choosedCourseName = item.CourseName;
                }
                foreach (var item in getNameTeacher)//get teacher name to use in print out
                {
                    dateOfGrade = (DateTime)item.dateOfGrade.GetValueOrDefault(DateTime.Today);
                    teacherName = item.teacherName;
                    teacherId = item.teacherId;
                }
                Console.WriteLine();
                AnsiConsole.MarkupLine("[green4]Please check that you entered the correct values?[/]");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(new string('-', 35));
                AnsiConsole.MarkupLine($"" +
                    $"[grey46]Student:[/] [yellow]{chosedStudentName}[/]\n" +
                    $"[grey46]ID:[/] [yellow]{selectedStudentId}[/]\n" +
                    $"[grey46]Course:[/] [yellow]{choosedCourseName}[/]\n" +
                    $"[grey46]Grade:[/] [yellow]{selectedGradeId}[/]");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine(new string('-', 35));
                Console.ResetColor();
                AnsiConsole.MarkupLine("[green4]is it right?[/] [grey46](Y/N)[/]");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.ResetColor();
                var checkInput = Console.ReadLine();
                if (checkInput.ToLower() == "y")
                {
                    try
                    {
                        string conString = "Data Source=ULLSTENLENOVO; Initial Catalog=School;Integrated Security=True";
                        using (SqlConnection connection = new SqlConnection(conString))
                        {
                            //SQL-Update code after input from user
                            SqlCommand cmd = new SqlCommand("UPDATE Exam SET DateOfGrade = GETDATE(), FK_GradeId = @selectedGradeId " +
                                "Where FK_StudentId = @selectedStudentId AND FK_CourseId = @selectedCourseId ", connection);
                            //open connection to base
                            connection.Open();
                            //set input value to sql-query
                            cmd.Parameters.AddWithValue("@selectedStudentId", selectedStudentId);
                            cmd.Parameters.AddWithValue("@selectedCourseId", selectedCourseId);
                            cmd.Parameters.AddWithValue("@selectedGradeId", selectedGradeId);

                            //Reads rows from table in data base
                            SqlDataReader sdr = cmd.ExecuteReader();
                            //Print out result
                            AnsiConsole.MarkupLine($"[mediumorchid1]Student[/] [darkorange3_1]{chosedStudentName}[/] [mediumorchid1]with ID:[/] [darkorange3_1]{selectedStudentId}[/] \n" +
                            $"[mediumorchid1]have completed the course[/] [darkorange3_1]{choosedCourseName}[/] [mediumorchid1]and got the grade[/] [darkorange3_1]{selectedGradeId}.[/] \n\n" +
                            $"[mediumorchid1]Signed by:[/] [chartreuse1]{teacherName}[/] [mediumorchid1]with ID[/] [darkorange3_1]{teacherId}[/]\n" +
                            $"[mediumorchid1]Date of Grade[/] [darkorange3_1]{dateOfGrade}[/]");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("OOPs, Somthing went wrong" + e);
                    }
                }
                else if (checkInput.ToLower() == "n")
                {
                    Console.WriteLine("Do you want to startover? (Y/N)");
                    var startover = Console.ReadLine();
                    if (startover.ToLower() == "y")
                    {
                        Console.Clear();
                        SaveCradeCourses();
                    }
                    else
                    {
                        Console.WriteLine("You will be sent to the menu again. Thank for this time!");
                        menu.AdminMenu();
                    }
                }
            }
            //Vi vill kunna spara ner betyg för en elev i varje kurs de läst och
            //vi vill kunna se vilken lärare som satt betyget.
            //Betyg ska också ha ett datum då de satts. (SQL i SSMS)
        }
        public void StoredProcedurId() //OK 
        {
            var totalStudents = 0; //store totalStudents here
            using (var context = new SchoolContext()) //Count total students in school to use as lenght when select id to show
            {
                totalStudents = context.Students.Count();
            }
            Menu menu = new Menu();
            ShowStudents(); //Show student to choose from
            Console.WriteLine();
            AnsiConsole.Markup("[deepskyblue2]Select the ID of the student you want to get information about: [/] ");
            int InputId = 0;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out InputId) && InputId >= 1 && InputId <= totalStudents)
                {
                    break;
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Select student by ID![/]");
                }
            }
            string conString = "Data Source=ULLSTENLENOVO; Initial Catalog=School;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(conString))
            {
                //SQL-code
                SqlCommand cmd = new SqlCommand("StudentInfoById", connection);
                //Sets commandtyp to call for stored
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //open connection to base
                connection.Open();
                cmd.Parameters.AddWithValue("@Id", InputId); //Send parameter to sql query
                SqlDataReader sdr = cmd.ExecuteReader();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(new string('-', 133));
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("| {0, -2} | {1, -18} | {2, -3} | {3, -15} | {4, -6} | {5, -17} | {6, -12} | {7, -14} | {8, -10} | {9, -5} | ", "Id", "Name", "Age", "Security number", "Gender", "Street", "Postal code", "City", "Homeland", "Class");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(new string('-', 133));
                Console.ForegroundColor = ConsoleColor.Green;
                while (sdr.Read()) //loop to show information
                {
                     Console.WriteLine("| {0, -2} | {1, -18} | {2, -3} | {3, -15} | {4, -6} | {5, -17} | {6, -12} | {7, -14} | {8, -10} | {9, -5} |", sdr["StudentId"], sdr["Name"], sdr["Age"], sdr["Security Number"], sdr["Gender"], sdr["Street"], sdr["Postal Code"], sdr["City"], sdr["Homeland"], sdr["Class"]);
                }
              
                Console.WriteLine(new string('-', 133));
                Console.ResetColor();
                Console.WriteLine();
                AnsiConsole.MarkupLine("[yellow]Do you want to search for another student id?[/] [deepskyblue2](Y/N)[/]");
                var serachAgain = Console.ReadLine();
                if (serachAgain.ToLower() == "y")
                {
                    Console.Clear();
                    StoredProcedurId();
                }
                else
                {
                    AnsiConsole.MarkupLine("[green3_1]Come back Monday - Friday[/] [yellow](07-16)[/][green3_1]if you need more information.[/] ");
                    menu.AdminMenu();
                }
                sdr.Close(); //Close readeing from database
            }
        }
        public void SetGradeTransaction() //FIXXA SQL KODEN TILL UPDATE ISTÄLLLET FÖR INSERT INTO
        {
            Menu menu = new Menu();
            using (var context = new SchoolContext())
            {
                //local variables to store inputs
                int selectedCourseId = 0;
                int setStaffAdminId = 0;
                var studentName = "";
                var courseName = "";
                var tempTeacherName = "";
                var teacherName = "";
                int tempTeacherId = 0;
                int teacherId = 0;

                ShowStudentsNoGrade(); //Show students to choose from
                int selectedStudentId;
                AnsiConsole.Markup("[green3]Enter student ID:[/] ");
                while (true)
                {
                    var getStudents = from s in context.Students select s;
                    var getStudentId = Console.ReadLine();
                    if (int.TryParse(getStudentId, out selectedStudentId))
                    {
                        foreach (var s in getStudents)
                        {
                            if (selectedStudentId == s.StudentId) //save picked student name for later use
                            {
                                studentName = s.FirstName + " " + s.LastName;
                            }
                        }
                        break;
                    }
                    else
                    {
                        AnsiConsole.Markup("[red]Select a student ID![/] ");
                    }
                }
                    Console.WriteLine();
                    ShowCourses(selectedStudentId); //Show courses to choose from with StudentId as input to show courses that not student have
                    AnsiConsole.Markup("[green3_1]Select course by ID:[/] ");
                    var courseInput = Console.ReadLine();
                    while (true)
                    {
                        if (int.TryParse(courseInput, out selectedCourseId) && selectedCourseId >= 1 && selectedCourseId <= 6)
                        {
                            var getCourses = from c in context.Courses select c;
                            foreach (var c in getCourses)
                            {
                                if (selectedCourseId.ToString() == c.CourseId.ToString()) //save course name for later use
                                {
                                    courseName = c.CourseName;
                                }
                            }
                            break;
                        }
                        else
                        {
                        AnsiConsole.Markup("[red]Select course 1-6![/] ");
                        }
                    }
                    switch (selectedCourseId) //Set right teacher to course
                    {
                        case 1:
                            setStaffAdminId = 4;
                            break;
                        case 2:
                            setStaffAdminId = 5;
                            break;
                        case 3:
                            setStaffAdminId = 6;
                            break;
                        case 4:
                            setStaffAdminId = 7;
                            break;
                        case 5:
                            setStaffAdminId = 15;
                            break;
                        case 6:
                            setStaffAdminId = 20;
                            break;
                    } //Set teacher to course
                Console.WriteLine();
                AnsiConsole.Markup("[green1]What grade has the student received?[/][grey](1-5):[/] ");
                var gradeInput = Console.ReadLine();
                int selectedGrade = 0;
                while (true)
                {
                    if (int.TryParse(gradeInput, out selectedGrade) && selectedGrade >= 1 && selectedGrade <= 5)
                    {
                        break;
                    }
                    else
                    {
                        AnsiConsole.Markup("[red]Select grade 1-5![/] ");
                    }
                }
                //Get name of teacher to use in output later
                var getTeacher = from sa in context.StaffAdmins
                                 join s in context.staff on sa.FkStaffId equals s.StaffId
                                 where sa.StaffAdminId == setStaffAdminId
                                 select new
                                 {
                                     tempTeacherId = sa.StaffAdminId,
                                     tempTeacherName = s.FirstName + " " + s.LastName,
                                 };
                foreach (var teacher in getTeacher)
                {
                    teacherId = teacher.tempTeacherId;
                    teacherName = teacher.tempTeacherName;
                }
                //Declared returnd rows to 0
                int returnRows = 0;
                string conString = "Data Source=ULLSTENLENOVO; Initial Catalog=School;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    //string with query
                    string SqlInsertGrade2 = $"UPDATE Exam SET DateOfGrade = GETDATE(), FK_GradeId = {selectedGrade} " +
                        $"Where FK_StudentId = {selectedStudentId}, AND FK_CourseId = {selectedCourseId}, FK_StaffAdminId = {setStaffAdminId}";

                    //Works with INSERT INTO but tha make new row, not update
                    //string SqlInsertGrade = "INSERT INTO Exam (DateOfGrade, FK_StudentId, FK_CourseId, FK_GradeId, FK_StaffAdminId)\r\n\t\t" +
                    //    $"VALUES (GETDATE(), {selectedStudentId}, {selectedCourseId}, {selectedGrade}, {setStaffAdminId})";
                    //Open connection do database
                    connection.Open();

                    SqlCommand command = connection.CreateCommand();  // Enlist a command in the current transaction
                    SqlTransaction sqlTran = connection.BeginTransaction();   // Start a local transaction.

                    command.Transaction = sqlTran;
                    try
                    {
                        // Execute command with string query
                        command.CommandText = SqlInsertGrade2;

                        returnRows = command.ExecuteNonQuery(); //Get affected rows 

                        //command.Parameters.Add("@selectedGrade", SqlDbType.Int).Value = selectedGrade;
                        //command.Parameters.Add("@selectedStudentId", SqlDbType.Int).Value = selectedStudentId;
                        //command.Parameters.Add("@selectedCourseId", SqlDbType.Int).Value = selectedCourseId;
                        //command.Parameters.Add("@setStaffAdminId", SqlDbType.Int).Value = setStaffAdminId;
                        //returns rows affected

                        //Commit the transaction.
                        sqlTran.Commit();

                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(new string('-', 53));
                        Console.ResetColor();
                        //Print result if transactions went well.
                        AnsiConsole.MarkupLine($"[mediumorchid1]Student[/] [darkorange3_1]{studentName}[/] [mediumorchid1]with ID:[/] [darkorange3_1]{selectedStudentId},[/] \n" +
                            $"[mediumorchid1]have completed the course[/] [darkorange3_1]{courseName}[/] [mediumorchid1]and got the grade[/] [darkorange3_1]{selectedGrade},[/] \n" +
                            $"[mediumorchid1]Signed by:[/] [chartreuse1]{teacherName}[/] [mediumorchid1]with ID[/] [chartreuse1]{teacherId}.[/]");
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        AnsiConsole.MarkupLine(new string('-', 53));
                        Console.ResetColor();
                        MessageByGrade(selectedGrade, studentName);
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
                            sqlTran.Rollback(); // Attempt to roll back the transaction.
                        }
                        catch (Exception exRollback)
                        {
                            // Throws an InvalidOperationException if the connection
                            // is closed or the transaction has already been rolled
                            // back on the server.
                            Console.WriteLine(exRollback.Message);
                        }
                    }
                    sqlTran.Dispose();
                }
            }
        } //OK
        public void ShowClasses() //internal method to show classes in school in method student in class
        {
            using (var context = new SchoolContext())
            {
                var getClass = from c in context.Classes
                                  orderby c.ClassId 
                                  select c;

                Console.WriteLine(new string('-', 14));
                AnsiConsole.MarkupLine("| [green]{0, -2}[/] | [green]{1, -5}[/] |", "Id", "Class");
                Console.WriteLine(new string('-', 14));
                foreach (var c in getClass)
                {
                    AnsiConsole.MarkupLine("| [grey46]{0, -2}[/] | [yellow]{1, -5}[/] | ", c.ClassId, c.ClassName);
                }
                Console.WriteLine(new string('-', 14));
            }
        } //OK
        public void ShowCoursesActiveNotActive() //OK
        {
            Menu menu = new Menu();  
            AnsiConsole.MarkupLine("[lightsalmon3_1]Do you want to see[/][chartreuse2] (A)ctive[/] [lightsalmon3_1]or[/] [red](N)ot Active[/] [lightsalmon3_1]courses?[/]");
            var input = Console.ReadLine();
            var choosedStatus = "";
            if (input.ToLower() == "a")
            {
                choosedStatus = "Active";
            }
            else if (input.ToLower() == "n")
            {
                choosedStatus = "NotActive";
            }

            using (var context = new SchoolContext())
            {
                var course = from c in context.Courses
                             where c.CourseStatus == choosedStatus
                             orderby c.CourseId
                             select c;
                Console.WriteLine(new string('-', 47));
                AnsiConsole.MarkupLine("| [paleturquoise4]{0, -2}[/] | [paleturquoise4]{1, -25}[/] | [paleturquoise4]{2, -10}[/] |", "ID", "Course", "Status");
                Console.WriteLine(new string('-', 47));
                foreach (var c in course)
                {
                    if (c.CourseStatus == "Active")
                    {
                        AnsiConsole.MarkupLine("| [grey39]{0, -2}[/] | [grey39]{1, -25}[/] | [yellow]{2, -10}[/] |", c.CourseId, c.CourseName, c.CourseStatus);
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("| [grey39]{0, -2}[/] | [grey39]{1, -25}[/] | [red]{2, -10}[/] |", c.CourseId, c.CourseName, c.CourseStatus);
                    }
                   
                }
                Console.WriteLine(new string('-', 47));
            }
            Console.WriteLine();      
            while (true)
            {
                AnsiConsole.MarkupLine("[chartreuse4]Do you want to search again?[/] [yellow2](Y/N)[/]");
                var searchAgain = Console.ReadLine();
                if (searchAgain.ToLower() == "y")
                {
                    Console.Clear();
                    ShowCoursesActiveNotActive();
                    break;
                }
                else if (searchAgain.ToLower() == "n")
                {
                    Console.WriteLine("You are moved to the student menu again.");
                    Thread.Sleep(1000);
                    Console.Clear();
                    menu.AdminMenu();
                    break;
                }
            }     
        }
        public void ShowStudentExtraInfo()
        {
            var totalStudents = 0; //store totalStudents here
            int selectedStudentId = 0; //Store selected student id
            int selectedCourseId = 0; //Store selected student id
            using (var context = new SchoolContext()) //Count total students in school to use as lenght when select id to show
            {
                totalStudents = context.Students.Count();
            }
            Menu menu = new Menu();
            ShowStudents(); //Show student to choose from
            Console.WriteLine();
            AnsiConsole.Markup("[deepskyblue2]Select your ID to get your school information.[/] ");

            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out selectedStudentId) && selectedStudentId >= 1 && selectedStudentId <= totalStudents)
                {
                    break;
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Select your ID![/]");
                }
            }
            string conString = "Data Source=ULLSTENLENOVO; Initial Catalog=School;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(conString))
            {
                
                //SQL-code
                SqlCommand cmd = new SqlCommand("ExtraStudentInfoById", connection);
                //Sets commandtyp to call for stored
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                //open connection to base
                connection.Open();
                cmd.Parameters.AddWithValue("@selectedStudentId", selectedStudentId); //Send parameter to sql query
                SqlDataReader sdr = cmd.ExecuteReader();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(new string('-', 84));
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("| {0, -2} | {1, -18} | {2, -5} | {3, -18} | {4, -6} | {5, -18}|", "Id", "Student name", "Class", "Course", "Grade", "Teacher in course");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(new string('-', 84));
                Console.ForegroundColor = ConsoleColor.Green;
                var noGrade = "no grade";
                while (sdr.Read()) //loop to show information
                {
                    if (sdr["Grade"].ToString() == "99")
                    {
                        
                    }
                    Console.WriteLine("| {0, -2} | {1, -18} | {2, -5} | {3, -18} | {4, -6} | {5, -18}|", sdr["ID"], sdr["Student"], sdr["Class"], sdr["Course"], sdr["Grade"], sdr["Teacher"]);
                }

                Console.WriteLine(new string('-', 84));
                Console.ResetColor();
                Console.WriteLine();
                AnsiConsole.MarkupLine("[yellow]Do you want to search for another student id?[/] [deepskyblue2](Y/N)[/]");
                var serachAgain = Console.ReadLine();
                if (serachAgain.ToLower() == "y")
                {
                    Console.Clear();
                    StoredProcedurId();
                }
                else
                {
                    AnsiConsole.MarkupLine("[green3_1]Come back Monday - Friday[/] [yellow](07-16)[/][green3_1]if you need more information.[/] ");
                    menu.AdminMenu();
                }
                sdr.Close(); //Close readeing from database
            }
        }
        private void SaveMoreStudent() //OK Internal method
        {
            Menu menu = new Menu();
            Console.WriteLine("Are there more employees to be registered today? [Y/N]");
            var moreAdd = Console.ReadLine();
            if (moreAdd.ToLower() == "y")
            {
                EnrollmentStudent();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("You find out the way out yourself");
                Console.ResetColor();
                menu.AdminMenu();
            }
        }
        public void ShowCourses(int selectedStudentId) //OK
        {
            using (var context = new SchoolContext())
            {
                //Code to get courses that student alreade have
                var takenCourses = from e in context.Exams
                                   join c in context.Courses on e.FkCourseId equals c.CourseId
                                   where e.FkStudentId == selectedStudentId
                                   select new { c.CourseId, c.CourseName } into x
                                   group x by new { x.CourseId, x.CourseName } into g
                                   select new
                                   {
                                       courseId = g.Key.CourseId,
                                       CourseName = g.Key.CourseName
                                   };

                string[] courstaken = new string[4]; //Declare array size
                int i = 0;
                var tempCourse1 = "";
                var tempCourse2 = "";
                var tempCourse3 = "";
                var tempCourse4 = "";
                foreach (var item in takenCourses) //loop to collect courses to array
                {
                    courstaken[i++] = item.CourseName; //saves courses with grades that the student has
                }
                //Assigned tempCourse 1-4 courses from array.  
                tempCourse1 = courstaken[0];
                tempCourse2 = courstaken[1];
                tempCourse3 = courstaken[2];
                tempCourse4 = courstaken[3];
                //Show courses, but not courses the student is already taking.
                var getCourses = from c in context.Courses
                                 where c.CourseStatus == "Active"
                                 where c.CourseName != tempCourse1
                                 where c.CourseName != tempCourse2
                                 where c.CourseName != tempCourse3
                                 where c.CourseName != tempCourse4
                                 orderby c.CourseId
                                 select c;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(new string('-', 25));
                Console.ResetColor();
                AnsiConsole.MarkupLine("[grey46]|[/] [violet]{0, -2}[/] [grey46]|[/] [grey46]{1, -16}[/] [grey46]|[/]", "ID", "Course");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(new string('-', 25));
                Console.ResetColor();
                foreach (var c in getCourses)
                {
                    AnsiConsole.MarkupLine("[grey46]|[/] [violet]{0, -2}[/] [grey46]|[/] [grey46]{1, -16}[/] [grey46]|[/]", c.CourseId, c.CourseName);
                }
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(new string('-', 25));
                Console.ResetColor();
            }
        }
        public void ShowCoursesStudent(int selectedStudentId, out int totalCoursesStudent) 
        {
            using (var context = new SchoolContext()) //Print students courses where no grade is set.
            {
                totalCoursesStudent = 0;
                var getStudentCourses = from e in context.Exams
                                        join c in context.Courses on e.FkCourseId equals c.CourseId
                                        join g in context.Grades on e.FkGradeId equals g.GradeId
                                        where e.FkStudentId == selectedStudentId
                                        where e.FkGradeId == 6 //6 is no grade
                                        select new
                                        {
                                            c.CourseId,
                                            c.CourseName
                                        };
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(new string('-', 24));
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("| {0, -2} | {1, -15} |", "ID", "Course");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(new string('-', 24));
                Console.ResetColor();
                foreach (var item in getStudentCourses)
                {
                    totalCoursesStudent += item.CourseId;
                    AnsiConsole.MarkupLine("| [lightskyblue3]{0, -2}[/] | [lightskyblue3]{1, -15}[/] |", item.CourseId, item.CourseName);
                }
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(new string('-', 24));
                Console.ResetColor();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
        }
        public void ShowActiveCourses() //OK
        {
            Menu menu = new Menu();
            using (var context = new SchoolContext())
            {
                var course = from c in context.Courses
                             where c.CourseStatus == "Active"
                             orderby c.CourseId
                             select c;
                Console.WriteLine(new string('-', 47));
                AnsiConsole.MarkupLine("| [paleturquoise4]{0, -2}[/] | [paleturquoise4]{1, -25}[/] |", "ID", "Course");
                Console.WriteLine(new string('-', 47));
                foreach (var c in course)
                {
                    if (c.CourseStatus == "Active")
                    {
                        AnsiConsole.MarkupLine("| [grey39]{0, -2}[/] | [grey39]{1, -25}[/] |", c.CourseId, c.CourseName);
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("| [grey39]{0, -2}[/] | [grey39]{1, -25}[/] |", c.CourseId, c.CourseName);
                    }

                }
                Console.WriteLine(new string('-', 47));
            }
        }
        private void ShowStudents() //OK Internal method
        {
            GetStudentListSpinner();
            Console.Clear();
            using(var context = new SchoolContext())
            {
                var showStudents = from s in context.Students select s;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                AnsiConsole.MarkupLine(new string('-', 30));
                Console.ResetColor();
                AnsiConsole.MarkupLine("[orange1]|[/] [grey46]{0, -3}[/] [orange1]|[/] [grey46]{1, -20}[/] [orange1]|[/]", "ID", "Name");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                AnsiConsole.MarkupLine(new string('-', 30));
                Console.ResetColor();
                foreach (var s in showStudents)  //Show all students to select from
                {
                    AnsiConsole.MarkupLine("[orange1]|[/] [yellow]{0, -3}[/] [orange1]|[/] [grey46]{1, -20}[/] [orange1]|[/]", s.StudentId, s.FirstName + " " + s.LastName);
                }
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                AnsiConsole.MarkupLine(new string('-', 30));
                Console.ResetColor();
            }
        }
        public void ShowStudentsNoGrade() //OK Internal method
        {
            GetStudentListSpinner();
            Console.Clear();
            using (var context = new SchoolContext())
            {
                var showStudents = from e in context.Exams
                                   join s in context.Students on e.FkStudentId equals s.StudentId
                                   where e.FkGradeId == 6
                                   select new { s.StudentId, s.FirstName, s.LastName } into x
                                   group x by new { x.StudentId, x.FirstName, x.LastName } into g
                                   select new
                                   {
                                       studentId = g.Key.StudentId,
                                       firstName = g.Key.FirstName,
                                       lastName = g.Key.LastName
                                   };
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                AnsiConsole.MarkupLine(new string('-', 30));
                Console.ResetColor();
                AnsiConsole.MarkupLine("[orange1]|[/] [grey46]{0, -3}[/] [orange1]|[/] [grey46]{1, -20}[/] [orange1]|[/]", "ID", "Name");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                AnsiConsole.MarkupLine(new string('-', 30));
                Console.ResetColor();
                foreach (var s in showStudents)  //Show all students to select from
                {
                    //AnsiConsole.MarkupLine("[orange1]|[/] [yellow]{0, -3}[/] [orange1]|[/] [grey46]{1, -20}[/] [orange1]|[/]", s.StudentId, s.FirstName + " " + s.LastName);
                    AnsiConsole.MarkupLine("[orange1]|[/] [yellow]{0, -3}[/] [orange1]|[/] [grey46]{1, -20}[/] [orange1]|[/]", s.studentId, s.firstName + " " + s.lastName);
                }
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                AnsiConsole.MarkupLine(new string('-', 30));
                Console.ResetColor();
            }
        }
        private void SeeOtherClass() //internal method for show student in class
        {
            Menu menu = new Menu();
            if (!AnsiConsole.Confirm("Do you want see student from another class?")) //Prompt for new serch or not
            {
                Console.Clear();
                menu.AdminMenu();
            }
            else
            {
                Console.Clear();
                ShowStudentInClass();
            }
        } //OK Internal method
        private void MessageByGrade(int selectedGrade, string studentName) //OK Internal method
        {
            switch (selectedGrade)
            {
                case 1:
                    AnsiConsole.MarkupLine($"[red]Grade {selectedGrade} ??[/] [cyan1]{studentName}[/][darkmagenta_1], you need to put in more effort to pass the semester![/]");
                    break;
                case 2:
                    AnsiConsole.MarkupLine($"[red1]Grade {selectedGrade} ??[/] [cyan1]{studentName}[/][darkmagenta_1], this time it was on the limit! \n" +
                        $"Don´t make it unnecessarily exciting if you want to pass the course![/]");
                    break;
                case 3:
                    AnsiConsole.MarkupLine($"[chartreuse3]Grade {selectedGrade} ??[/] [cyan1]{studentName}[/][darkmagenta_1], You are average and can do better if you want![/]");
                    break;
                case 4:
                    AnsiConsole.MarkupLine($"[silver]Grade {selectedGrade}![/] [cyan1]{studentName}[/][darkmagenta_1], you are better than 75% of them at school! Good work![/]");
                    break;
                case 5:
                    AnsiConsole.MarkupLine($"[gold3_1]Grade {selectedGrade}![/] [cyan1]{studentName}[/][darkmagenta_1], right on target! Notice that you have put in the necessary time! Great job![/]");
                    break;
            }

        }
        private void GetStudentListSpinner() //OK Internal method for show student by id procedure
        {
            AnsiConsole.Status()
    .Start("[grey69]Retrieving...[/]", ctx =>
    {
        // Simulate some work
        AnsiConsole.MarkupLine("[green]Retrieving the student list[/][darkorange]...[/]");
        Thread.Sleep(1000);
        AnsiConsole.MarkupLine("[lime]Damn computer, so slow[/][orange1]...[/]");
        Thread.Sleep(1000);
        AnsiConsole.MarkupLine("[indianred1_1]I have asked for a new one, but will not experience a new one here[/][orange1]...[/]");
        Thread.Sleep(1000);

        // Update the status and spinner
        ctx.Status("Thinking some more");
        ctx.Spinner(Spinner.Known.Moon);
        ctx.SpinnerStyle(Style.Parse("green"));

        // Simulate some work
        AnsiConsole.MarkupLine("[lightgoldenrod1]Finally, here is the list![/]");
        //Thread.Sleep(1000);
         });

        }
    }
}
