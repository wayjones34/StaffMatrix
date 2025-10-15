using Newtonsoft.Json;
using StaffMatrix.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace StaffMatrix.Data
{
    public static class JsonDataHelper
    {
        private static readonly string employeeFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "employees.json");
        private static readonly string shiftFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "shifts.json");

        // ✅ Load Employees
        public static List<Employee> LoadEmployees()
        {
            try
            {
                if (!File.Exists(employeeFile))
                    return new List<Employee>();

                string json = File.ReadAllText(employeeFile);
                return JsonConvert.DeserializeObject<List<Employee>>(json) ?? new List<Employee>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading employees: " + ex.Message);
                return new List<Employee>();
            }
        }

        // ✅ Save Employees
        public static void SaveEmployees(List<Employee> employees)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(employeeFile)); // Ensure "Data" folder exists
                string json = JsonConvert.SerializeObject(employees, Formatting.Indented);
                File.WriteAllText(employeeFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving employees: " + ex.Message);
            }
        }

        // ✅ Generate Unique Employee ID
        /// <summary>
        /// Generates a unique Employee ID by loading existing employees and finding the next available ID.
        /// This method ensures uniqueness even when called from different parts of the application.
        /// </summary>
        /// <returns>A unique Employee ID</returns>
        public static int GenerateUniqueEmployeeID()
        {
            var employees = LoadEmployees();

            if (employees.Count == 0)
                return 1;

            int maxId = 0;
            foreach (var employee in employees)
            {
                if (employee.EmployeeID > maxId)
                    maxId = employee.EmployeeID;
            }

            return maxId + 1;
        }

        // ✅ Load Shifts
        public static List<Shift> LoadShifts()
        {
            try
            {
                if (!File.Exists(shiftFile))
                    return new List<Shift>();

                string json = File.ReadAllText(shiftFile);
                return JsonConvert.DeserializeObject<List<Shift>>(json) ?? new List<Shift>();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading shifts: " + ex.Message);
                return new List<Shift>();
            }
        }

        // ✅ Save Shifts
        public static void SaveShifts(List<Shift> shifts)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(shiftFile)); // Ensure "Data" folder exists
                string json = JsonConvert.SerializeObject(shifts, Formatting.Indented);
                File.WriteAllText(shiftFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving shifts: " + ex.Message);
            }
        }

        // ✅ Generate Unique Shift ID
        /// <summary>
        /// Generates a unique Shift ID by loading existing shifts and finding the next available ID.
        /// This method ensures uniqueness even when called from different parts of the application.
        /// </summary>
        /// <returns>A unique Shift ID</returns>
        public static int GenerateUniqueShiftID()
        {
            var shifts = LoadShifts();

            if (shifts.Count == 0)
                return 1;

            int maxId = 0;
            foreach (var shift in shifts)
            {
                if (shift.ShiftID > maxId)
                    maxId = shift.ShiftID;
            }

            return maxId + 1;
        }
    }
}
