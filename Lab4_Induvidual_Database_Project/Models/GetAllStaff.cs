using System;
using System.Collections.Generic;

namespace Lab4_Induvidual_Database_Project.Models
{
    public partial class GetAllStaff
    {
        public int StaffId { get; set; }
        public string Name { get; set; } = null!;
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? Ssn { get; set; }
        public string? DateOfEmployment { get; set; }
        public string? EmployedYear { get; set; }
        public string Position { get; set; } = null!;
    }
}
