using System;
using System.Collections.Generic;

namespace Lab4_Induvidual_Database_Project.Models
{
    public partial class Student
    {
        public Student()
        {
            Exams = new HashSet<Exam>();
        }

        public int StudentId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DayOfBirth { get; set; }
        public int? Age { get; set; }
        public string? SecurityNumber { get; set; }
        public string? Gender { get; set; }
        public int? FkAddressId { get; set; }
        public int? FkClassId { get; set; }

        public virtual Address? FkAddress { get; set; }
        public virtual Class? FkClass { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
    }
}
