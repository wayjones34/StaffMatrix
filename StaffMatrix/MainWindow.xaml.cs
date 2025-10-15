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

namespace StaffMatrix
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnEmployeeManagement_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new EmployeeManagementView();
        }

        private void BtnShiftScheduling_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ShiftSchedulingView();
        }

        private void BtnShiftCalendar_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new ShiftCalendarView();
        }
    }
}
