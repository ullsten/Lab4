using System;
using System.Collections.Generic;

namespace Lab4_Induvidual_Database_Project.Models
{
    public partial class StaffAdmin
    {
        public StaffAdmin()
        {
            Exams = new HashSet<Exam>();
            PayrollOffices = new HashSet<PayrollOffice>();
        }

        public int StaffAdminId { get; set; }
        public int? FkStaffId { get; set; }
        public int? FkPositionId { get; set; }
        public int? FkAddressId { get; set; }

        public virtual Address? FkAddress { get; set; }
        public virtual Position? FkPosition { get; set; }
        public virtual staff? FkStaff { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
        public virtual ICollection<PayrollOffice> PayrollOffices { get; set; }
    }
}
