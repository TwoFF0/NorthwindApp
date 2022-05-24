namespace Northwind.Services.Employees
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a management service for employees.
    /// </summary>
    public interface IEmployeeManagementService
    {
        /// <summary>
        /// Shows a list of employees.
        /// </summary>
        /// <returns>A <see cref="IList{T}"/> of <see cref="Employee"/>.</returns>
        IAsyncEnumerable<Employee> ShowAllEmployeesAsync();

        /// <summary>
        /// Shows a list of employees using specified offset and limit for pagination.
        /// </summary>
        /// <param name="offset">An offset of the first element to return.</param>
        /// <param name="limit">A limit of elements to return.</param>
        /// <returns>A <see cref="IList{T}"/> of <see cref="Employee"/>.</returns>
        IAsyncEnumerable<Employee> ShowEmployeesAsync(int offset, int limit);

        /// <summary>
        /// Try to show a employee with specified identifier.
        /// </summary>
        /// <param name="employeeId">A employee identifier.</param>
        /// <returns>Returns true if a employee <see cref="Employee"/> is returned; otherwise false.</returns>
        Task<Employee> ShowEmployeeAsync(int employeeId);

        /// <summary>
        /// Creates a new employee.
        /// </summary>
        /// <param name="employee">A <see cref="Employee"/> to create.</param>
        /// <returns>An identifier of a created employee.</returns>
        Task<int> CreateEmployeeAsync(Employee employee);

        /// <summary>
        /// Updates a employee.
        /// </summary>
        /// <param name="employee">A <see cref="Employee"/>.</param>
        /// <param name="employeeId">A employee identifier.</param>
        /// <returns>True if a employee is updated; otherwise false.</returns>
        Task<bool> UpdateEmployeeAsync(Employee employee, int employeeId);

        /// <summary>
        /// Destroys an existed employee.
        /// </summary>
        /// <param name="employeeId">A employee identifier.</param>
        /// <returns>True if a product category is destroyed; otherwise false.</returns>
        Task<bool> DeleteEmployeeAsync(int employeeId);
    }
}
