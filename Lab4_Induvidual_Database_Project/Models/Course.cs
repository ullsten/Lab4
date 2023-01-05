using System;
using System.Collections.Generic;

namespace Lab4_Induvidual_Database_Project.Models
{
    public partial class Course
    {
        public Course()
        {
            Exams = new HashSet<Exam>();
        }

        public int CourseId { get; set; }
        public string CourseName { get; set; } = null!;
        public string? CourseStatus { get; set; }

        public virtual ICollection<Exam> Exams { get; set; }
    }
}
