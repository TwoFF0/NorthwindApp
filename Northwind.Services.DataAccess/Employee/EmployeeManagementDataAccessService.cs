#pragma warning disable CA2007
#pragma warning disable SA1210
#pragma warning disable SA1600

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Northwind.DataAccess.DAO_s;
using Northwind.DataAccess.TransferObjects;

namespace Northwind.Services.DataAccess.Employee
{
    using Northwind.DataAccess;
    using Northwind.Services.Employees;

    public class EmployeeManagementDataAccessService : IEmployeeManagementService
    {
        private readonly IEmployeeDAO employeeDAO;

        public EmployeeManagementDataAccessService(NorthwindDataAccessFactory factory)
        {
            var fact = factory ?? throw new ArgumentNullException(nameof(factory));
            this.employeeDAO = fact.GetEmployeeDataAccessObject();
        }

        public async Task<int> CreateEmployeeAsync(Employees.Employee employee)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            return await this.employeeDAO.InsertEmployeeAsync(ToDto(employee));
        }

        public async Task<bool> DeleteEmployeeAsync(int employeeId) => await this.employeeDAO.DeleteEmployeeAsync(employeeId);

        public async IAsyncEnumerable<Employees.Employee> ShowAllEmployeesAsync()
        {
            await foreach (var employee in this.employeeDAO.SelectAllEmployeeAsync())
            {
                yield return ToObject(employee);
            }
        }

        public async Task<Employees.Employee> ShowEmployeeAsync(int employeeId) => ToObject(await this.employeeDAO.FindEmployeeAsync(employeeId));

        public async IAsyncEnumerable<Employees.Employee> ShowEmployeesAsync(int offset, int limit)
        {
            await foreach (var employee in this.employeeDAO.SelectEmployeesAsync(offset, limit))
            {
                yield return ToObject(employee);
            }
        }

        public async Task<bool> UpdateEmployeeAsync(Employees.Employee employee, int employeeId)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            employee.Id = employeeId;

            return await this.employeeDAO.UpdateEmployeeAsync(ToDto(employee));
        }

        private static Employees.Employee ToObject(EmployeeTransferObject employee)
        {
            return new Employees.Employee()
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Address = employee.Address,
                City = employee.City,
                TitleOfCourtesy = employee.TitleOfCourtesy,
                Country = employee.Country,
                Extension = employee.Extension,
                ReportsTo = employee.ReportsTo,
                Notes = employee.Notes,
                HireDate = employee.HireDate,
                BirthDate = employee.BirthDate,
                Photo = employee.Photo,
                PhotoPath = employee.PhotoPath,
                HomePhone = employee.HomePhone,
                PostalCode = employee.PostalCode,
                Region = employee.Region,
                Title = employee.Title,
            };
        }

        private static EmployeeTransferObject ToDto(Employees.Employee employee)
        {
            return new EmployeeTransferObject()
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Address = employee.Address,
                City = employee.City,
                TitleOfCourtesy = employee.TitleOfCourtesy,
                Country = employee.Country,
                Extension = employee.Extension,
                ReportsTo = employee.ReportsTo,
                Notes = employee.Notes,
                HireDate = employee.HireDate,
                BirthDate = employee.BirthDate,
                Photo = employee.Photo,
                PhotoPath = employee.PhotoPath,
                HomePhone = employee.HomePhone,
                PostalCode = employee.PostalCode,
                Region = employee.Region,
                Title = employee.Title,
            };
        }
    }
}
