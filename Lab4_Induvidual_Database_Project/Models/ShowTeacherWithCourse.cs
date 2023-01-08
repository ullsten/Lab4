using System;
using System.Collections.Generic;

namespace Lab4_Induvidual_Database_Project.Models
{
    public partial class ShowTeacherWithCourse
    {
        public string Name { get; set; } = null!;
        public string Position { get; set; } = null!;
        public int Id { get; set; }
        public string AssignedCourse { get; set; } = null!;
    }
}
