using System;
using System.Collections.Generic;

namespace Lab4_Induvidual_Database_Project.Models
{
    public partial class staff
    {
        public int StaffId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? DayOfBirth { get; set; }
        public int? Age { get; set; }
        public string? SecurityNumber { get; set; }
        public string? Gender { get; set; }
        public DateTime? HireDate { get; set; }
        public string? YearOnSchool { get; set; }
    }
}
