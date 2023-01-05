using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Lab4_Induvidual_Database_Project;
using Lab4_Induvidual_Database_Project.Data;
using Lab4_Induvidual_Database_Project.Models;
using Microsoft.Data.SqlClient;
using Spectre.Console;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Color = Spectre.Console.Color;

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
            ShowCoursesActiveNotActive(); //Show courses to choose from, shows only active courses
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
                    Console.WriteLine("Select position 1-6!");
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
            //Generate random grade for new student.
            Random random = new Random();
            int randomGradeId = random.Next(1, 5);
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
                        "INSERT INTO Exam(DateOfGrade, FK_StudentId, FK_CourseId, FK_GradeId, FK_StaffAdminId)\r\n" +
                        "VALUES (DATEADD(day, -15, GETDATE()), IDENT_CURRENT('Student'), @courseId, @randomGradeId, @staffadminid)\r\n" +
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
                    cmd.Parameters.AddWithValue("@randomGradeId", randomGradeId);
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
        public void ShowStudentInClass()
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
        } //OK
        public void SaveCradeCourses()
        {
            //Vi vill kunna spara ner betyg för en elev i varje kurs de läst och
            //vi vill kunna se vilken lärare som satt betyget.
            //Betyg ska också ha ett datum då de satts. (SQL i SSMS)


            Console.WriteLine("Save grade in courses");
        }
        public void StoredProcedurId() //OK Change some colors only and maybe add information?
        {
            Menu menu = new Menu();
            Console.WriteLine("Enter studentId: ");
            var InputId = Console.ReadLine();
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
                Console.WriteLine(new string('-', 125));
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("| {0, -2} | {1, -15} | {2, -3} | {3, -15} | {4, -6} | {5, -17} | {6, -12} | {7, -10} | {8, -8} | {9, -6} | ", 
                    "Id", "Name", "Age", "Security number", "Gender", "Street", "Postal code", "City", "Homeland", "Class");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(new string('-', 125));

                Console.ForegroundColor = ConsoleColor.Green;
                while (sdr.Read()) //loop to show information
                {
                    Console.WriteLine("| {0, -2} | {1, -15} | {2, -3} | {3, -15} | {4, -6} | {5, -17} | {6, -12} | {7, -10} | {8, -8} | {9, -6} |",
                        sdr["StudentId"], sdr["Name"], sdr["Age"], sdr["Security Number"], sdr["Gender"], sdr["Street"], sdr["Postal Code"], sdr["City"], sdr["Homeland"], sdr["Class"]);
                }
                Console.WriteLine(new string('-', 125));
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("Do you want to search for information on another ID? [Y/N]");
                var serachAgain = Console.ReadLine();
                if (serachAgain.ToLower() == "y")
                {
                    Console.Clear();
                    StoredProcedurId();
                }
                else
                {
                    Console.WriteLine("Come back Monday - Friday if you need more information. ");
                    menu.AdminMenu();
                }
                sdr.Close(); //Close readeing from database
            }
        }
        public void SetGradeTransaction()
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
                ShowStudents(); //Show students to choose from
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
                    }
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
                    string SqlInsertGrade = "INSERT INTO Exam (DateOfGrade, FK_StudentId, FK_CourseId, FK_GradeId, FK_StaffAdminId)\r\n\t\t" +
                        $"VALUES (GETDATE(), {selectedStudentId}, {selectedCourseId}, {selectedGrade}, {setStaffAdminId})";
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
                        command.CommandText = SqlInsertGrade;
                        //returns rows affected
                        returnRows = command.ExecuteNonQuery();
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
        } //OK
        public void MessageByGrade(int selectedGrade, string studentName) //OK
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
        private void SeeOtherClass() //internal method for show student in class
        {
            Menu menu = new Menu();
            if (!AnsiConsole.Confirm("Do you want see student from another class?")) //Prompt for new serch or not
            {
                Console.Clear();
                menu.PupilMenu();
            }
            else
            {
                Console.Clear();
                ShowStudentInClass();
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
        public void ShowCoursesActiveNotActive() //Fix output color mm.
        {
            Menu menu = new Menu();  
            Console.WriteLine("Do you want to see [A]ctive or [N]onActive courses?");
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
                foreach (var c in course)
                {
                    Console.WriteLine($"{c.CourseId} {c.CourseName} {c.CourseStatus}");
                }
            }
            Console.WriteLine();
            Console.WriteLine("Do you want to search again? [Y/N]");
            var searchAgain = Console.ReadLine();
            if (searchAgain.ToLower() == "y")
            {
                Console.Clear();
                ShowCoursesActiveNotActive();
            }
            else
            {
                Console.WriteLine("You are moved to the student menu again.");
                Thread.Sleep(1000);
                Console.Clear();
                menu.AdminMenu();
            }
        } 
        private void SaveMoreStudent() //OK
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
                //Assigned tempCourse 1-4 courses fron array.  
                tempCourse1 = courstaken[0];
                tempCourse2 = courstaken[1];
                tempCourse3 = courstaken[2];
                tempCourse4 = courstaken[3];
                //Show courses to choose, dosnt show courses student is taking
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
        public void ShowStudents() //OK
        {
            using(var context = new SchoolContext())
            {
                var showStudents = from s in context.Students select s;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                AnsiConsole.MarkupLine(new string('-', 32));
                Console.ResetColor();
                AnsiConsole.MarkupLine("[orange1]|[/] [grey46]{0, -3}[/] [orange1]|[/] [grey46]{1, -20}[/] [orange1]|[/]", "ID", "Name");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                AnsiConsole.MarkupLine(new string('-', 32));
                Console.ResetColor();
                foreach (var s in showStudents)  //Show all students to select from
                {
                    AnsiConsole.MarkupLine("[orange1]|[/] [yellow]{0, -3}[/] [orange1]|[/] [grey46]{1, -20}[/] [orange1]|[/]", s.StudentId, s.FirstName + " " + s.LastName);
                }
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                AnsiConsole.MarkupLine(new string('-', 32));
                Console.ResetColor();
            }
        }
    }
}
