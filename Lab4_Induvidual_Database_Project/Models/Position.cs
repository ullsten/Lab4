using System;
using System.Collections.Generic;

namespace Lab4_Induvidual_Database_Project.Models
{
    public partial class Position
    {
        public Position()
        {
            StaffAdmins = new HashSet<StaffAdmin>();
        }

        public int PositionId { get; set; }
        public string PositionName { get; set; } = null!;

        public virtual ICollection<StaffAdmin> StaffAdmins { get; set; }
    }
}
