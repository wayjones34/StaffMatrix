using System;
using System.Collections.Generic;
using StaffMatrix.Data;
using StaffMatrix.Models;

namespace StaffMatrix
{
    /// <summary>
    /// Simple test class to verify Employee ID uniqueness functionality.
    /// This can be run as a console application or called from Main for testing.
    /// </summary>
    public static class EmployeeIDTest
    {
        public static void RunTests()
        {
            Console.WriteLine("=== Employee ID Uniqueness Test ===");
            
            // Test 1: Generate ID when no employees exist
            Console.WriteLine("\nTest 1: Generate ID with empty employee list");
            var emptyEmployees = new List<Employee>();
            JsonDataHelper.SaveEmployees(emptyEmployees);
            int firstId = JsonDataHelper.GenerateUniqueEmployeeID();
            Console.WriteLine($"Expected: 1, Actual: {firstId}, Result: {(firstId == 1 ? "PASS" : "FAIL")}");
            
            // Test 2: Generate ID with existing employees
            Console.WriteLine("\nTest 2: Generate ID with existing employees");
            var existingEmployees = new List<Employee>
            {
                new Employee { EmployeeID = 1, FirstName = "John", LastName = "Doe", Role = "Admin", Email = "john@test.com", Phone = "123-456-7890" },
                new Employee { EmployeeID = 2, FirstName = "Jane", LastName = "Smith", Role = "User", Email = "jane@test.com", Phone = "098-765-4321" }
            };
            JsonDataHelper.SaveEmployees(existingEmployees);
            int nextId = JsonDataHelper.GenerateUniqueEmployeeID();
            Console.WriteLine($"Expected: 3, Actual: {nextId}, Result: {(nextId == 3 ? "PASS" : "FAIL")}");
            
            // Test 3: Generate ID with non-sequential existing IDs
            Console.WriteLine("\nTest 3: Generate ID with non-sequential existing IDs");
            var nonSequentialEmployees = new List<Employee>
            {
                new Employee { EmployeeID = 1, FirstName = "Alice", LastName = "Johnson", Role = "Admin", Email = "alice@test.com", Phone = "111-222-3333" },
                new Employee { EmployeeID = 5, FirstName = "Bob", LastName = "Wilson", Role = "User", Email = "bob@test.com", Phone = "444-555-6666" },
                new Employee { EmployeeID = 3, FirstName = "Charlie", LastName = "Brown", Role = "User", Email = "charlie@test.com", Phone = "777-888-9999" }
            };
            JsonDataHelper.SaveEmployees(nonSequentialEmployees);
            int maxPlusOneId = JsonDataHelper.GenerateUniqueEmployeeID();
            Console.WriteLine($"Expected: 6, Actual: {maxPlusOneId}, Result: {(maxPlusOneId == 6 ? "PASS" : "FAIL")}");
            
            // Test 4: Multiple consecutive ID generations
            Console.WriteLine("\nTest 4: Multiple consecutive ID generations");
            JsonDataHelper.SaveEmployees(existingEmployees); // Reset to employees with IDs 1,2
            int id1 = JsonDataHelper.GenerateUniqueEmployeeID(); // Should be 3
            
            // Simulate adding the employee with the generated ID
            existingEmployees.Add(new Employee { EmployeeID = id1, FirstName = "Test1", LastName = "User1", Role = "Test", Email = "test1@test.com", Phone = "000-000-0001" });
            JsonDataHelper.SaveEmployees(existingEmployees);
            
            int id2 = JsonDataHelper.GenerateUniqueEmployeeID(); // Should be 4
            Console.WriteLine($"First ID - Expected: 3, Actual: {id1}, Result: {(id1 == 3 ? "PASS" : "FAIL")}");
            Console.WriteLine($"Second ID - Expected: 4, Actual: {id2}, Result: {(id2 == 4 ? "PASS" : "FAIL")}");
            
            Console.WriteLine("\n=== Test Complete ===");
        }
    }
}
