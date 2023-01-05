using System;
using System.Collections.Generic;

namespace Lab4_Induvidual_Database_Project.Models
{
    public partial class Grade
    {
        public Grade()
        {
            Exams = new HashSet<Exam>();
        }

        public int GradeId { get; set; }
        public int GradeName { get; set; }

        public virtual ICollection<Exam> Exams { get; set; }
    }
}
