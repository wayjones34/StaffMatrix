using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffMatrix.Models
{
    public class Shift
    {
        public int ShiftID { get; set; }
        public int EmployeeID { get; set; }
        public DateTime ShiftDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Location { get; set; }

        // For display purposes - not stored in JSON
        public string EmployeeName { get; set; }
    }
}
