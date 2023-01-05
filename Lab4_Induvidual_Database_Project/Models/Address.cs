using System;
using System.Collections.Generic;

namespace Lab4_Induvidual_Database_Project.Models
{
    public partial class Address
    {
        public Address()
        {
            StaffAdmins = new HashSet<StaffAdmin>();
            Students = new HashSet<Student>();
        }

        public int AddressId { get; set; }
        public string StreetAddress { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string City { get; set; } = null!;
        public string? Homeland { get; set; }

        public virtual ICollection<StaffAdmin> StaffAdmins { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
