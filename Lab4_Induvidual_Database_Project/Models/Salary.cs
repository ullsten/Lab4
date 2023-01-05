using System;
using System.Collections.Generic;

namespace Lab4_Induvidual_Database_Project.Models
{
    public partial class Salary
    {
        public Salary()
        {
            PayrollOffices = new HashSet<PayrollOffice>();
        }

        public int SalaryId { get; set; }
        public string? SalaryName { get; set; }
        public int? Amount { get; set; }
        public string? SalaryType { get; set; }

        public virtual ICollection<PayrollOffice> PayrollOffices { get; set; }
    }
}
