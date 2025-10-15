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
using StaffMatrix.Data;

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
                _shifts = JsonDataHelper.LoadShifts();
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
                _employees = JsonDataHelper.LoadEmployees();
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
                        // Find the employee by EmployeeID
                        var employee = _employees.FirstOrDefault(emp => emp.EmployeeID == shift.EmployeeID);

                        string employeeName = employee?.FullName ?? $"Employee ID: {shift.EmployeeID}";

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


    }
}