using System;
using System.Collections.Generic;

namespace Lab4_Induvidual_Database_Project.Models
{
    public partial class Salary
    {
        public Salary()
        {
            StaffAdmins = new HashSet<StaffAdmin>();
        }

        public int SalaryId { get; set; }
        public string? SalaryType { get; set; }

        public virtual ICollection<StaffAdmin> StaffAdmins { get; set; }
    }
}
