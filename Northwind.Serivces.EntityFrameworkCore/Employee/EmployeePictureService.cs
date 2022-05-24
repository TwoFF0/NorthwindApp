#pragma warning disable SA1600
#pragma warning disable CA2007
#pragma warning disable CA2208

namespace Northwind.Serivces.EntityFrameworkCore.Employee
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Northwind.Services.Employees;

    public class EmployeePictureService : IEmployeePictureService
    {
        private readonly NorthwindContext context;

        public EmployeePictureService(NorthwindContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public async Task<byte[]> ShowPhotoAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(id)} cannot be less or equal zero");
            }

            var employee = await this.context.Employees.FindAsync(id);

            return employee?.Photo;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdatePhotoAsync(int id, Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(id)} cannot be less or equal zero");
            }

            var employee = await this.context.Employees.FindAsync(id);

            if (employee != null)
            {
                await using var memoryStream = (MemoryStream)stream;
                byte[] picWrapped = new byte[memoryStream.Length];
                Array.Copy(memoryStream.ToArray(), 0, picWrapped, 0, memoryStream.Length);

                employee.Photo = picWrapped;
                await this.context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyPhotoAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(id)} cannot be less or equal zero");
            }

            var employee = await this.context.Employees.FindAsync(id);

            if (employee != null)
            {
                employee.Photo = null;
                await this.context.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
