using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using StaffMatrix.Models; // Ensure Employee.cs is in StaffMatrix.Models
using StaffMatrix.Data; // For JsonDataHelper

namespace StaffMatrix
{
    public partial class EmployeeManagementView : UserControl
    {
        private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "employees.json");
        private ObservableCollection<Employee> _employees = new ObservableCollection<Employee>();

        public EmployeeManagementView()
        {
            InitializeComponent();
            employeeGrid.ItemsSource = _employees;
            LoadEmployees();
        }

        // Add Employee
        private void AddEmployee_Click(object sender, RoutedEventArgs args)
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtRole.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Please complete all fields.", "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var emp = new Employee
            {
                EmployeeID = JsonDataHelper.GenerateUniqueEmployeeID(),
                FirstName = txtFirstName.Text.Trim(),
                LastName = txtLastName.Text.Trim(),
                Role = txtRole.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Phone = txtPhone.Text.Trim()
            };

            _employees.Add(emp);
            SaveEmployees();
            ClearInputs();
        }

        // Remove Employee (button inside DataGrid row)
        private void RemoveEmployee_Click(object sender, RoutedEventArgs args)
        {
            if (sender is Button btn && btn.Tag is Employee emp)
            {
                _employees.Remove(emp);
                SaveEmployees();
            }
        }

        // -------- Persistence --------

        private void LoadEmployees()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    // Ensure Data directory exists for first save
                    Directory.CreateDirectory(Path.GetDirectoryName(_filePath) ?? "Data");
                    return;
                }

                var json = File.ReadAllText(_filePath);
                var loaded = JsonConvert.DeserializeObject<ObservableCollection<Employee>>(json);

                _employees.Clear();
                if (loaded != null)
                {
                    foreach (var emp in loaded)
                        _employees.Add(emp);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employees: {ex.Message}", "Load Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveEmployees()
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_filePath) ?? "Data");
                var json = JsonConvert.SerializeObject(_employees, Formatting.Indented);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving employees: {ex.Message}", "Save Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // -------- Helpers --------

        private void ClearInputs()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtRole.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
        }
    }
}
