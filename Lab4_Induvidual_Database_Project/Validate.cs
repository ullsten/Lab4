using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using Lab4_Induvidual_Database_Project.Data;

namespace Lab4_Induvidual_Database_Project
{
    internal class Validate
    {
        public void ValidateLastNumber()
        {
            using(var context = new SchoolContext())
            {
                while (true)
                {
                    var sNumber = "";
                    var ssn = from s in context.Students
                              select new
                              {
                                  lastssn = s.SecurityNumber.Substring(s.SecurityNumber.Length - 4, 4)
                              };
                    bool checkSsn = true;
                    while (true)
                    {
                        Console.WriteLine("Enter ssn number: ");
                        var ssnL = Console.ReadLine();
                        var last4 = ssnL.Substring(ssnL.Length - 4, 4);
                        foreach (var s in ssn)
                        {
                            if (last4 == s.lastssn)
                            {
                                checkSsn = false;
                                Console.WriteLine("SSN alreade exists, try again!");
                                continue;
                            }
                        }
                        if (checkSsn == true)
                        {
                            sNumber = ssnL;
                            break;
                        }
                    }
                    Console.WriteLine("SSN is Ok: " + sNumber);
                }
                
               
            }
           



            Console.ReadLine();
        }public void ValidateFirstNumber()
        {
            Console.WriteLine("Enter ssn number: ");
            var ssn = Console.ReadLine();
            var last4 = ssn.Substring(0,4);
            Console.WriteLine(last4);

            Console.ReadLine();
        }
    }
}
