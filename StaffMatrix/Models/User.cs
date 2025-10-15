using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffMatrix.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; } // For now, plain text (we’ll hash later)
        public string Role { get; set; }     // e.g., Admin, Supervisor, etc.
    }
}