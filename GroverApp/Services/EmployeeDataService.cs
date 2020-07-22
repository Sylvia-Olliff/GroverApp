using GroverApp.Repos;
using Microsoft.EntityFrameworkCore;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroverApp.Services
{
    public class EmployeeDataService : IDataService<Employee>
    {
        private EmployeesDatabaseContext _dbContext;
        private ILogger _logger;

        public ObservableCollection<Employee> DataSet { get; }

        public EmployeeDataService(EmployeesDatabaseContext context)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _dbContext = context;
            _dbContext.Database.EnsureCreated();
            _dbContext.Employees.Load();
            _dbContext.Employees.OrderBy(emp => emp.Name);
            DataSet = _dbContext.Employees.Local.ToObservableCollection();
        }

        private bool IsValid(Employee entity)
        {
            return !string.IsNullOrEmpty(entity.Name) && !string.IsNullOrEmpty(entity.JobTitle);
        }

        public bool Add(Employee entity)
        {
            if (_dbContext.Employees.Any(emp => emp.Id == entity.Id) || !IsValid(entity))
                return false;

            try
            {
                if (entity.Id == 0)
                    entity.Id = DataSet.Count + 1;

                _dbContext.Employees.Add(entity);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error Adding employee Id: {entity.Id} Name: {entity.Name} Job Title: {entity.JobTitle} !");
                return false;
            }
        }

        public bool Delete(Employee entity)
        {
            var employee = _dbContext.Employees.FirstOrDefault(emp => emp.Id == entity.Id);

            if (employee == null)
                return true;

            try
            {
                _dbContext.Remove(employee);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error Deleting employee Id: {entity.Id} Name: {entity.Name} Job Title: {entity.JobTitle} !");
                return false;
            }
        }

        public void Load()
        {
            if (_dbContext.Employees.Count() == 0)
            {
                _dbContext.Employees.AddRange(new Employee[]
                {
                    new Employee { Name = "John Doe", JobTitle = "Awesome Manager" },
                    new Employee { Name = "Jane Doe", JobTitle = "Coolness Team Leader"}
                });
            }
        }

        public bool Update(Employee entity)
        {
            var employee = _dbContext.Employees.FirstOrDefault(emp => emp.Id == entity.Id);

            if (employee == null || !IsValid(entity))
                return false;

            try
            {
                // maintain object reference from context.
                employee.Name = entity.Name;
                employee.JobTitle = entity.JobTitle;

                _dbContext.Employees.Update(employee);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Error updating to employee Id: {entity.Id} Name: {entity.Name} Job Title: {entity.JobTitle}, from Id: {employee.Id} Name: {employee.Name} Job Title: {employee.JobTitle} !");
                return false;
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
