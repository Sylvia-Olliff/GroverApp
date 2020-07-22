using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroverApp.Repos
{
    public class EmployeesDatabaseContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public EmployeesDatabaseContext(DbContextOptions<EmployeesDatabaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(GetEmployees()); //If no db file, create it with some test data
            base.OnModelCreating(modelBuilder);
        }

        private Employee[] GetEmployees()
        {
            return new Employee[]
                {
                    new Employee { Id = 1, Name = "John Doe", JobTitle = "Awesome Manager" },
                    new Employee { Id = 2, Name = "Jane Doe", JobTitle = "Coolness Team Leader"}
                };
        }
    }
}
