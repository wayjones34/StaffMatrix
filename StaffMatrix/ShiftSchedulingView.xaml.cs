using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using StaffMatrix.Models;

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
                if (File.Exists(employeeFilePath))
                {
                    string json = File.ReadAllText(employeeFilePath);
                    var employeeList = JsonConvert.DeserializeObject<List<Employee>>(json) ?? new List<Employee>();
                    employees.Clear();
                    foreach (var emp in employeeList)
                    {
                        employees.Add(emp);
                    }
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
                if (File.Exists(shiftFilePath))
                {
                    string json = File.ReadAllText(shiftFilePath);
                    var shiftList = JsonConvert.DeserializeObject<List<Shift>>(json) ?? new List<Shift>();
                    Shifts.Clear();
                    foreach (var shift in shiftList)
                    {
                        // Populate EmployeeName for display
                        var employee = employees.FirstOrDefault(e => e.EmployeeID == shift.EmployeeID);
                        shift.EmployeeName = employee?.FullName ?? $"Employee ID: {shift.EmployeeID}";
                        Shifts.Add(shift);
                    }
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
                    ShiftID = Shifts.Count + 1, // Simple ID generation
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
                var json = JsonConvert.SerializeObject(Shifts, Formatting.Indented);
                Directory.CreateDirectory("Data");
                File.WriteAllText(shiftFilePath, json);
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
                if (File.Exists(shiftFilePath))
                {
                    var json = File.ReadAllText(shiftFilePath);
                    var loadedShifts = JsonConvert.DeserializeObject<ObservableCollection<Shift>>(json);

                    Shifts.Clear();
                    foreach (var shift in loadedShifts)
                        Shifts.Add(shift);

                    MessageBox.Show("Shifts loaded successfully!", "Loaded", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("No saved shifts found.", "Load", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading shifts: " + ex.Message);
            }
        }
    }


}
