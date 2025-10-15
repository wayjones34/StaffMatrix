using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffMatrix.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }   // required by XAML & code-behind
        public string LastName { get; set; }   // required
        public string Role { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }   // required

        public string FullName => $"{FirstName} {LastName}";
    }
}
