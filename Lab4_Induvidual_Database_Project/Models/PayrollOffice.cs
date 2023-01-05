using System;
using System.Collections.Generic;

namespace Lab4_Induvidual_Database_Project.Models
{
    public partial class PayrollOffice
    {
        public int PayrollOfficeId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? FkSalaryId { get; set; }
        public int? FkStaffAdminId { get; set; }

        public virtual Salary? FkSalary { get; set; }
        public virtual StaffAdmin? FkStaffAdmin { get; set; }
    }
}
