using GroverApp.Repos;
using GroverApp.Services;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace GroverApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Snackbar Snackbar;

        private readonly EmployeeDataService _dbService;
        private Employee _newEmployee = new Employee();
        private Employee _selectedEmployee = null;

        public MainWindow(EmployeeDataService dataService)
        {
            _dbService = dataService;
            InitializeComponent();
            RefreshEmployeesList();
            NewEmployeeGrid.DataContext = _newEmployee;

            // Does this count as the obligitory "Hello World"?
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2500);
            }).ContinueWith(t =>
            {
                MainSnackbar.MessageQueue.Enqueue("Welcome to the Grover Employee Management App");
            }, TaskScheduler.FromCurrentSynchronizationContext());

            Snackbar = this.MainSnackbar;
        }

        private void RefreshEmployeesList()
        {
            EmployeeDG.ItemsSource = _dbService.DataSet;
        }

        private void AddItem(object sender, RoutedEventArgs e)
        {
            if (!_dbService.Add(_newEmployee))
                Snackbar.MessageQueue.Enqueue("Failed to Add Employee!");
            else
                Snackbar.MessageQueue.Enqueue("Employee successfully added!");

            _newEmployee = new Employee();
            NewEmployeeGrid.DataContext = _newEmployee;
            RefreshEmployeesList();
        }

        private void UpdateItem(object sender, RoutedEventArgs e)
        {
            if (_selectedEmployee == null)
                return;

            if (!_dbService.Update(_selectedEmployee))
                Snackbar.MessageQueue.Enqueue("Failed to Update Employee!");
            else
                Snackbar.MessageQueue.Enqueue("Employee successfully updated!");
            RefreshEmployeesList();
        }

        private void SelectEmployeeToEdit(object sender, RoutedEventArgs e)
        {
            _selectedEmployee = (sender as FrameworkElement).DataContext as Employee;
            UpdateEmployeeGrid.DataContext = _selectedEmployee;
        }

        private void DeleteEmployee(object sender, RoutedEventArgs e)
        {
            var employeeToDelete = (sender as FrameworkElement).DataContext as Employee;
            if (!_dbService.Delete(employeeToDelete))
                Snackbar.MessageQueue.Enqueue("Failed to Delete Employee!");
            else
                Snackbar.MessageQueue.Enqueue("Employee successfully deleted!");
            RefreshEmployeesList();
        }
    }
}
