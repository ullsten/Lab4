using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Lab4_Induvidual_Database_Project
{
    public class Update
    {
        public void SetDayOfBirth() //Update Age for last added student
        {
            string conSetDayOfBirth = "Data Source=ULLSTENLENOVO; Initial Catalog=School;Integrated Security=True";
            using (SqlConnection updateAge = new SqlConnection(conSetDayOfBirth))
            {
                SqlCommand cmdAge = new SqlCommand("UPDATE Staff SET DayOfBirth = SUBSTRING(SecurityNumber, 3,6) FROM Staff", updateAge);
                //Open connection
                updateAge.Open();
                SqlDataReader sdr2 = cmdAge.ExecuteReader();
            }
        }
        public void AgeUpdate() //Update gender for last added student
        {
            string conAgeUpdate = "Data Source=ULLSTENLENOVO; Initial Catalog=School;Integrated Security=True";
            using (SqlConnection updateAge = new SqlConnection(conAgeUpdate))
            {
                SqlCommand cmd3 = new SqlCommand("UPDATE Staff SET Age = DATEDIFF(year,DayOfBirth,GETDATE()) Where StaffId = IDENT_CURRENT('Staff')", updateAge);
                //open connection
                updateAge.Open();
                SqlDataReader sdr = cmd3.ExecuteReader();
            }
        }
        public void GenderUpdate() //Update gender for last added student
        {
            string conUpdateSsn = "Data Source=ULLSTENLENOVO; Initial Catalog=School;Integrated Security=True";
            using (SqlConnection updateSsn = new SqlConnection(conUpdateSsn))
            {
                SqlCommand cmd3 = new SqlCommand("UPDATE Staff\r\nSET Gender = (CASE WHEN right(rtrim(SecurityNumber),1) IN ('1', '3', '5', '7', '9') THEN 'Male'\r\n" +
                "WHEN right(rtrim(SecurityNumber), 1) IN ('2', '4', '6', '8', '0') THEN 'Female' END)\r\n" +
                "Where StaffId = IDENT_CURRENT('Staff')", updateSsn);
                //open connection
                updateSsn.Open();
                SqlDataReader sdr = cmd3.ExecuteReader();
            }
        }
        public void YearOnSchoolUpdate() //Update gender for last added student
        {
            string conYearOnSchool = "Data Source=ULLSTENLENOVO; Initial Catalog=School;Integrated Security=True";
            using (SqlConnection updateYear = new SqlConnection(conYearOnSchool))
            {
                SqlCommand cmd3 = new SqlCommand("UPDATE Staff SET YearOnSchool = DATEDIFF(year,HireDate,GETDATE())", updateYear);
                //open connection
                updateYear.Open();
                SqlDataReader sdr = cmd3.ExecuteReader();
            }
        }

    }
}
