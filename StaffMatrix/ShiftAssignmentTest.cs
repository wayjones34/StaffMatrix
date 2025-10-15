using System;
using System.Collections.Generic;
using System.IO;
using StaffMatrix.Data;
using StaffMatrix.Models;

namespace StaffMatrix
{
    /// <summary>
    /// Test class to verify the robust shift assignment implementation
    /// </summary>
    public static class ShiftAssignmentTest
    {
        public static void RunTests()
        {
            Console.WriteLine("=== Shift Assignment Implementation Tests ===\n");

            // Test 1: Unique Shift ID Generation
            TestUniqueShiftIDGeneration();

            // Test 2: Shift Persistence
            TestShiftPersistence();

            // Test 3: Employee Name Resolution
            TestEmployeeNameResolution();

            Console.WriteLine("\n=== All Tests Completed ===");
        }

        private static void TestUniqueShiftIDGeneration()
        {
            Console.WriteLine("Test 1: Unique Shift ID Generation");

            // Clear existing shifts for clean test
            JsonDataHelper.SaveShifts(new List<Shift>());

            // Test empty list
            int id1 = JsonDataHelper.GenerateUniqueShiftID();
            Console.WriteLine($"Empty list - Expected: 1, Actual: {id1}, Result: {(id1 == 1 ? "PASS" : "FAIL")}");

            // Create some test shifts
            var testShifts = new List<Shift>
            {
                new Shift { ShiftID = 1, EmployeeID = 1, ShiftDate = DateTime.Today, StartTime = TimeSpan.FromHours(9), EndTime = TimeSpan.FromHours(17), Location = "Office" },
                new Shift { ShiftID = 2, EmployeeID = 2, ShiftDate = DateTime.Today, StartTime = TimeSpan.FromHours(10), EndTime = TimeSpan.FromHours(18), Location = "Office" }
            };
            JsonDataHelper.SaveShifts(testShifts);

            // Test with existing shifts
            int id2 = JsonDataHelper.GenerateUniqueShiftID();
            Console.WriteLine($"With existing shifts - Expected: 3, Actual: {id2}, Result: {(id2 == 3 ? "PASS" : "FAIL")}");

            // Test non-sequential IDs
            var nonSequentialShifts = new List<Shift>
            {
                new Shift { ShiftID = 1, EmployeeID = 1, ShiftDate = DateTime.Today, StartTime = TimeSpan.FromHours(9), EndTime = TimeSpan.FromHours(17), Location = "Office" },
                new Shift { ShiftID = 5, EmployeeID = 2, ShiftDate = DateTime.Today, StartTime = TimeSpan.FromHours(10), EndTime = TimeSpan.FromHours(18), Location = "Office" },
                new Shift { ShiftID = 3, EmployeeID = 3, ShiftDate = DateTime.Today, StartTime = TimeSpan.FromHours(11), EndTime = TimeSpan.FromHours(19), Location = "Office" }
            };
            JsonDataHelper.SaveShifts(nonSequentialShifts);

            int id3 = JsonDataHelper.GenerateUniqueShiftID();
            Console.WriteLine($"Non-sequential IDs - Expected: 6, Actual: {id3}, Result: {(id3 == 6 ? "PASS" : "FAIL")}");

            Console.WriteLine();
        }

        private static void TestShiftPersistence()
        {
            Console.WriteLine("Test 2: Shift Persistence");

            // Create test shifts
            var testShifts = new List<Shift>
            {
                new Shift { ShiftID = 1, EmployeeID = 1, ShiftDate = DateTime.Today, StartTime = TimeSpan.FromHours(9), EndTime = TimeSpan.FromHours(17), Location = "Main Office" },
                new Shift { ShiftID = 2, EmployeeID = 2, ShiftDate = DateTime.Today.AddDays(1), StartTime = TimeSpan.FromHours(10), EndTime = TimeSpan.FromHours(18), Location = "Branch Office" }
            };

            // Save shifts
            JsonDataHelper.SaveShifts(testShifts);

            // Load shifts
            var loadedShifts = JsonDataHelper.LoadShifts();

            bool persistenceTest = loadedShifts.Count == 2 && 
                                 loadedShifts[0].ShiftID == 1 && 
                                 loadedShifts[1].ShiftID == 2 &&
                                 loadedShifts[0].Location == "Main Office" &&
                                 loadedShifts[1].Location == "Branch Office";

            Console.WriteLine($"Shift persistence - Expected: True, Actual: {persistenceTest}, Result: {(persistenceTest ? "PASS" : "FAIL")}");
            Console.WriteLine();
        }

        private static void TestEmployeeNameResolution()
        {
            Console.WriteLine("Test 3: Employee Name Resolution");

            // Create test employees
            var testEmployees = new List<Employee>
            {
                new Employee { EmployeeID = 1, FirstName = "John", LastName = "Doe", Role = "Manager", Email = "john@test.com", Phone = "555-0001" },
                new Employee { EmployeeID = 2, FirstName = "Jane", LastName = "Smith", Role = "Developer", Email = "jane@test.com", Phone = "555-0002" }
            };
            JsonDataHelper.SaveEmployees(testEmployees);

            // Load employees
            var loadedEmployees = JsonDataHelper.LoadEmployees();

            // Test employee lookup by ID
            var employee1 = loadedEmployees.FirstOrDefault(e => e.EmployeeID == 1);
            var employee2 = loadedEmployees.FirstOrDefault(e => e.EmployeeID == 2);
            var employeeNotFound = loadedEmployees.FirstOrDefault(e => e.EmployeeID == 999);

            bool nameResolutionTest = employee1?.FullName == "John Doe" && 
                                    employee2?.FullName == "Jane Smith" && 
                                    employeeNotFound == null;

            Console.WriteLine($"Employee name resolution - Expected: True, Actual: {nameResolutionTest}, Result: {(nameResolutionTest ? "PASS" : "FAIL")}");

            // Test fallback for missing employee
            string fallbackName = employeeNotFound?.FullName ?? "Employee ID: 999";
            bool fallbackTest = fallbackName == "Employee ID: 999";
            Console.WriteLine($"Missing employee fallback - Expected: True, Actual: {fallbackTest}, Result: {(fallbackTest ? "PASS" : "FAIL")}");

            Console.WriteLine();
        }
    }
}
