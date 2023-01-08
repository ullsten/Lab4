using System;
using System.Collections.Generic;

namespace Lab4_Induvidual_Database_Project.Models
{
    public partial class Exam
    {
        public int ExamId { get; set; }
        public DateTime? StartDateCourse { get; set; }
        public int? FkStudentId { get; set; }
        public int? FkCourseId { get; set; }
        public int? FkGradeId { get; set; }
        public int? FkStaffAdminId { get; set; }
        public DateTime? DateOfGrade { get; set; }

        public virtual Course? FkCourse { get; set; }
        public virtual Grade? FkGrade { get; set; }
        public virtual StaffAdmin? FkStaffAdmin { get; set; }
        public virtual Student? FkStudent { get; set; }
    }
}
