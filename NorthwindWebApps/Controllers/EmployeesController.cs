#pragma warning disable CA2007

namespace NorthwindWebApps.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Northwind.Services.Employees;

    /// <summary>
    /// Controller of employees.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeManagementService employeeManagementService;
        private readonly IEmployeePictureService employeePictureService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeesController"/> class.
        /// </summary>
        /// <param name="employeeManagementService">EmployeeEntity service.</param>
        /// <param name="employeePictureService">EmployeeEntity picture service.</param>
        /// <exception cref="ArgumentNullException">Thrown if service is null.</exception>
        public EmployeesController(IEmployeeManagementService employeeManagementService, IEmployeePictureService employeePictureService)
        {
            this.employeeManagementService = employeeManagementService ?? throw new ArgumentNullException(nameof(employeeManagementService));
            this.employeePictureService = employeePictureService ?? throw new ArgumentNullException(nameof(employeePictureService));
        }

        /// <summary>
        /// Get all employees <see cref="Employee"/>.
        /// </summary>
        /// <returns>All existed employees.</returns>
        [HttpGet]
        public async IAsyncEnumerable<Employee> GetAllEmployeesAsync()
        {
            await foreach (var employee in this.employeeManagementService.ShowAllEmployeesAsync())
            {
                yield return employee;
            }
        }

        /// <summary>
        /// GET employees from <see cref="offset"/> to <see cref="limit"/>.
        /// </summary>
        /// <param name="offset">To start from.</param>
        /// <param name="limit">Limit of employees.</param>
        /// <returns>Numbers of employees in a range between <see cref="offset"/> + <see cref="limit"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <see cref="offset"/> or <see cref="limit"/> is invalid.</exception>
        [HttpGet("{offset}/{limit}")]
        public async IAsyncEnumerable<Employee> GetEmployeesAsync(int offset, int limit)
        {
            await foreach (var employee in this.employeeManagementService.ShowEmployeesAsync(offset, limit))
            {
                yield return employee;
            }
        }

        /// <summary>
        /// GET employee with specified <see cref="id"/>.
        /// </summary>
        /// <param name="id">Id of employee.</param>
        /// <returns>Specified employee.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeesAsync(int id)
        {
            var employee = await this.employeeManagementService.ShowEmployeeAsync(id);

            if (employee is null)
            {
                return this.NotFound();
            }

            return this.Ok(employee);
        }

        /// <summary>
        /// POST employee <see cref="Employee"/> to a collection.
        /// </summary>
        /// <param name="employee">employee to POST.</param>
        /// <returns>BadRequest if employee is null or <see cref="Employee"/> if everything is ok.</returns>
        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployeeAsync(Employee employee)
        {
            if (employee is null)
            {
                return this.BadRequest();
            }

            var id = await this.employeeManagementService.CreateEmployeeAsync(employee);

            if (id == -1)
            {
                return this.Conflict();
            }

            employee.Id = id;

            return this.CreatedAtAction(nameof(this.CreateEmployeeAsync), new { Id = id }, employee);
        }

        /// <summary>
        /// PUT new employee instead existed one.
        /// </summary>
        /// <param name="id">Id of employee to update.</param>
        /// <param name="employee">employee to update.</param>
        /// <returns>Bad request if category is null || NoContent -> everything OK || NotFound.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployeeAsync(int id, Employee employee)
        {
            if (employee is null)
            {
                return this.BadRequest();
            }

            var isUpdated = await this.employeeManagementService.UpdateEmployeeAsync(employee, id);

            if (!isUpdated)
            {
                return this.BadRequest();
            }

            return this.NoContent();
        }

        /// <summary>
        /// DELETE employee.
        /// </summary>
        /// <param name="id">Id of employee to delete.</param>
        /// <returns>Bad request if employee is null || NoContent -> everything OK || NotFound.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeAsync(int id)
        {
            var isDeleted = await this.employeeManagementService.DeleteEmployeeAsync(id);

            if (!isDeleted)
            {
                return this.NotFound();
            }

            return this.NoContent();
        }

        /// <summary>
        /// PUT photo.
        /// </summary>
        /// <param name="id">Id of picture to update.</param>
        /// <param name="photo">New picture.</param>
        /// <returns>BadRequest if something wrong and NoContent if all's good.</returns>
        [HttpPut("{id}/photo")]
        public async Task<IActionResult> UpdatePhotoAsync(int id, IFormFile photo)
        {
            if (photo is null)
            {
                return this.BadRequest();
            }

            await using var ms = new MemoryStream();
            await photo.CopyToAsync(ms);

            if (!await this.employeePictureService.UpdatePhotoAsync(id, ms))
            {
                return this.NotFound();
            }

            return this.NoContent();
        }

        /// <summary>
        /// GET picture of employee <see cref="id"/>.
        /// </summary>
        /// <param name="id">Id of employee which belongs to picture.</param>
        /// <returns>If picture is null -> NotFound || File if all's good.</returns>
        [HttpGet("{id}/photo")]
        public async Task<IActionResult> ShowPhotoAsync(int id)
        {
            var photo = await this.employeePictureService.ShowPhotoAsync(id);

            if (photo is null)
            {
                return this.NotFound();
            }

            return this.File(photo, "image/png");
        }

        /// <summary>
        /// DELETE picture.
        /// </summary>
        /// <param name="id">Id of picture to delete.</param>
        /// <returns>NoContent if all's good and BadRequest if bad.</returns>
        [HttpDelete("{id}/photo")]
        public async Task<IActionResult> DeletePhotoAsync(int id)
        {
            if (!await this.employeePictureService.DestroyPhotoAsync(id))
            {
                return this.NotFound();
            }

            return this.NoContent();
        }
    }
}
