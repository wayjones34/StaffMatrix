using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using StaffMatrix.Models;
using StaffMatrix.Data;

namespace StaffMatrix
{
    public partial class ShiftSchedulingView : UserControl
    {
        public ObservableCollection<Shift> Shifts { get; set; } = new ObservableCollection<Shift>();
        private ObservableCollection<Employee> employees = new ObservableCollection<Employee>();
        private readonly string shiftFilePath = "Data/shifts.json";
        private readonly string employeeFilePath = "Data/employees.json";

        public ShiftSchedulingView()
        {
            InitializeComponent();
            LoadEmployees();
            LoadShifts();
            shiftGrid.ItemsSource = Shifts;
            cmbEmployee.ItemsSource = employees;

        }

        private void LoadEmployees()
        {
            try
            {
                var employeeList = JsonDataHelper.LoadEmployees();
                employees.Clear();
                foreach (var emp in employeeList)
                {
                    employees.Add(emp);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading employees: " + ex.Message);
            }
        }

        private void LoadShifts()
        {
            try
            {
                var shiftList = JsonDataHelper.LoadShifts();
                Shifts.Clear();
                foreach (var shift in shiftList)
                {
                    // Populate EmployeeName for display
                    var employee = employees.FirstOrDefault(e => e.EmployeeID == shift.EmployeeID);
                    shift.EmployeeName = employee?.FullName ?? $"Employee ID: {shift.EmployeeID}";
                    Shifts.Add(shift);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading shifts: " + ex.Message);
            }
        }

        private void AddShift_Click(object sender, RoutedEventArgs e)
        {
            if (cmbEmployee.SelectedItem == null ||
                !shiftDate.SelectedDate.HasValue ||
                string.IsNullOrWhiteSpace(txtStartTime.Text) ||
                string.IsNullOrWhiteSpace(txtEndTime.Text))
            {
                MessageBox.Show("Please complete all fields.", "Missing Info", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var selectedEmployee = (Employee)cmbEmployee.SelectedItem;

                var shift = new Shift
                {
                    ShiftID = JsonDataHelper.GenerateUniqueShiftID(),
                    EmployeeID = selectedEmployee.EmployeeID,
                    ShiftDate = shiftDate.SelectedDate.Value,
                    StartTime = TimeSpan.Parse(txtStartTime.Text.Trim()),
                    EndTime = TimeSpan.Parse(txtEndTime.Text.Trim()),
                    Location = txtLocation?.Text?.Trim() ?? "Main Office",
                    EmployeeName = selectedEmployee.FullName // For display
                };

                Shifts.Add(shift);
                ClearForm();
                MessageBox.Show("Shift added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter valid time format (HH:MM).", "Invalid Time", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding shift: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearForm()
        {
            cmbEmployee.SelectedItem = null;
            shiftDate.SelectedDate = null;
            txtStartTime.Text = "09:00";
            txtEndTime.Text = "17:00";
            txtLocation.Text = "Main Office";
        }

        private void SaveToFile_Click(object sender, RoutedEventArgs e)
        {
            SaveShiftsToJson();
        }

        private void LoadFromFile_Click(object sender, RoutedEventArgs e)
        {
            LoadShiftsFromJson();
        }

        private void SaveShiftsToJson()
        {
            try
            {
                JsonDataHelper.SaveShifts(Shifts.ToList());
                MessageBox.Show("Shifts saved successfully!", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving shifts: " + ex.Message);
            }
        }

        private void LoadShiftsFromJson()
        {
            try
            {
                var loadedShifts = JsonDataHelper.LoadShifts();
                Shifts.Clear();
                foreach (var shift in loadedShifts)
                {
                    // Populate EmployeeName for display
                    var employee = employees.FirstOrDefault(e => e.EmployeeID == shift.EmployeeID);
                    shift.EmployeeName = employee?.FullName ?? $"Employee ID: {shift.EmployeeID}";
                    Shifts.Add(shift);
                }

                MessageBox.Show("Shifts loaded successfully!", "Loaded", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading shifts: " + ex.Message);
            }
        }

        private void EditShift_Click(object sender, RoutedEventArgs e)
        {
            var selectedShift = shiftGrid.SelectedItem as Shift;
            if (selectedShift == null)
            {
                MessageBox.Show("Please select a shift to edit.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Populate form with selected shift data
            var employee = employees.FirstOrDefault(emp => emp.EmployeeID == selectedShift.EmployeeID);
            cmbEmployee.SelectedItem = employee;
            shiftDate.SelectedDate = selectedShift.ShiftDate;
            txtStartTime.Text = selectedShift.StartTime.ToString(@"hh\:mm");
            txtEndTime.Text = selectedShift.EndTime.ToString(@"hh\:mm");
            txtLocation.Text = selectedShift.Location;

            // Remove the old shift from the collection
            Shifts.Remove(selectedShift);
        }

        private void RemoveShift_Click(object sender, RoutedEventArgs e)
        {
            var selectedShift = shiftGrid.SelectedItem as Shift;
            if (selectedShift == null)
            {
                MessageBox.Show("Please select a shift to remove.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to remove the shift for {selectedShift.EmployeeName} on {selectedShift.ShiftDate:MM/dd/yyyy}?",
                                       "Confirm Removal", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Shifts.Remove(selectedShift);
                JsonDataHelper.SaveShifts(Shifts.ToList());
                MessageBox.Show("Shift removed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }


}
