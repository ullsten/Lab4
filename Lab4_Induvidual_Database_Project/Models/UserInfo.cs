using System;
using System.Collections.Generic;

namespace Lab4_Induvidual_Database_Project.Models
{
    public partial class UserInfo
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? HashedPassword { get; set; }
        public string? Salt { get; set; }
    }
}
