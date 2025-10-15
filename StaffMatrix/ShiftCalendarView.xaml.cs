using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Newtonsoft.Json;
using StaffMatrix.Models;

namespace StaffMatrix
{
    public partial class ShiftCalendarView : UserControl
    {
        private List<Shift> _shifts;
        private List<Employee> _employees;
        private readonly string shiftFilePath = "Data/shifts.json";
        private readonly string employeeFilePath = "Data/employees.json";

        public ShiftCalendarView()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            LoadShifts();
            LoadEmployees();
        }

        private void LoadShifts()
        {
            try
            {
                if (File.Exists(shiftFilePath))
                {
                    string json = File.ReadAllText(shiftFilePath);
                    _shifts = JsonConvert.DeserializeObject<List<Shift>>(json) ?? new List<Shift>();
                }
                else
                {
                    _shifts = new List<Shift>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading shifts: " + ex.Message);
                _shifts = new List<Shift>();
            }
        }

        private void LoadEmployees()
        {
            try
            {
                if (File.Exists(employeeFilePath))
                {
                    string json = File.ReadAllText(employeeFilePath);
                    _employees = JsonConvert.DeserializeObject<List<Employee>>(json) ?? new List<Employee>();
                }
                else
                {
                    _employees = new List<Employee>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading employees: " + ex.Message);
                _employees = new List<Employee>();
            }
        }

        private void shiftCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            ShiftListBox.Items.Clear();

            if (shiftCalendar.SelectedDate.HasValue)
            {
                var selectedDate = shiftCalendar.SelectedDate.Value.Date;

                // Find shifts for the selected date
                var shiftsForDate = _shifts.Where(s => s.ShiftDate.Date == selectedDate).ToList();

                if (shiftsForDate.Any())
                {
                    foreach (var shift in shiftsForDate)
                    {
                        // Try to find the employee name
                        var employee = _employees.FirstOrDefault(emp => emp.FirstName + " " + emp.LastName == GetEmployeeName(shift.EmployeeID));

                        string employeeName = employee != null ? $"{employee.FirstName} {employee.LastName}" : $"Employee ID: {shift.EmployeeID}";

                        string shiftInfo = $"{employeeName} - {shift.StartTime:hh\\:mm} to {shift.EndTime:hh\\:mm}";
                        if (!string.IsNullOrEmpty(shift.Location))
                        {
                            shiftInfo += $" at {shift.Location}";
                        }

                        ShiftListBox.Items.Add(shiftInfo);
                    }
                }
                else
                {
                    ShiftListBox.Items.Add("No shifts scheduled for this date.");
                }
            }
        }

        private string GetEmployeeName(int employeeId)
        {
            // For now, since we don't have a proper employee ID system,
            // we'll just return a placeholder
            return $"Employee {employeeId}";
        }
    }
}