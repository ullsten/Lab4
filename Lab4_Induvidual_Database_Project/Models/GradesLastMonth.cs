using System;
using System.Collections.Generic;

namespace Lab4_Induvidual_Database_Project.Models
{
    public partial class GradesLastMonth
    {
        public string Student { get; set; } = null!;
        public string Course { get; set; } = null!;
        public int Grade { get; set; }
        public DateTime? DateOfGrade { get; set; }
    }
}
