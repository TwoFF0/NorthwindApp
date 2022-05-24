#pragma warning disable CA2007
#pragma warning disable SA1648

namespace Northwind.Serivces.EntityFrameworkCore.Employee
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Northwind.Services.Employees;

    /// <summary>
    /// Represents a management service for employees.
    /// </summary>
    public class EmployeeManagementService : IEmployeeManagementService
    {
        private readonly NorthwindContext context;

        /// <inheritdoc/>
        public EmployeeManagementService(NorthwindContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public async Task<int> CreateEmployeeAsync(Services.Employees.Employee employee)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            switch (employee.Id)
            {
                case < 0:
                    return -1;
                case 0:
                    employee.Id = this.context.Employees.Max(x => x.Id) + 1;
                    break;
            }

            await this.context.Employees.AddAsync(employee);
            await this.context.SaveChangesAsync();

            return employee.Id;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteEmployeeAsync(int employeeId)
        {
            var emp = await this.context.Employees.FindAsync(employeeId);

            if (emp is null)
            {
                return false;
            }

            this.context.Employees.Remove(emp);
            await this.context.SaveChangesAsync();

            return true;
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<Services.Employees.Employee> ShowAllEmployeesAsync()
        {
            await foreach (var employee in this.context.Employees)
            {
                yield return employee;
            }
        }

        /// <inheritdoc/>
        public async Task<Services.Employees.Employee> ShowEmployeeAsync(int employeeId) => await this.context.Employees.FindAsync(employeeId);

        /// <inheritdoc/>
        public async IAsyncEnumerable<Services.Employees.Employee> ShowEmployeesAsync(int offset, int limit)
        {
            var count = 0;

            await foreach (var employee in this.context.Employees)
            {
                if (count >= offset && count < offset + limit)
                {
                    yield return employee;
                }

                count++;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateEmployeeAsync(Services.Employees.Employee employee, int employeeId)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            var emp = await this.context.Employees.FindAsync(employeeId);

            if (emp is null)
            {
                return false;
            }

            emp.FirstName = employee.FirstName;
            emp.LastName = employee.LastName;
            emp.Address = employee.Address;
            emp.Country = employee.Country;
            emp.HireDate = employee.HireDate;
            emp.PostalCode = employee.PostalCode;
            emp.Title = employee.Title;
            emp.TitleOfCourtesy = employee.TitleOfCourtesy;
            emp.BirthDate = employee.BirthDate;
            emp.City = employee.City;
            emp.Extension = employee.Extension;
            emp.HomePhone = employee.HomePhone;
            emp.ReportsTo = employee.ReportsTo;
            emp.Photo = employee.Photo;
            emp.Region = employee.Region;
            emp.Notes = employee.Notes;
            emp.PhotoPath = employee.PhotoPath;

            await this.context.SaveChangesAsync();

            return true;
        }
    }
}
