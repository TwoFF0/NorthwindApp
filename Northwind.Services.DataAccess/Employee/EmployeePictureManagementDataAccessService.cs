namespace Northwind.Services.DataAccess.Employee
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Northwind.DataAccess;
    using Northwind.DataAccess.DAO_s;
    using Northwind.Services.Employees;

#pragma warning disable CA2007
#pragma warning disable SA1600

    public class EmployeePictureManagementDataAccessService : IEmployeePictureService
    {
        private readonly IEmployeePictureDAO employeePictureDao;

        public EmployeePictureManagementDataAccessService(NorthwindDataAccessFactory factory)
        {
            var fact = factory ?? throw new ArgumentNullException(nameof(factory));
            this.employeePictureDao = fact.GetEmployeePictureDataAccessObject();
        }

        public async Task<bool> DestroyPhotoAsync(int id) => await this.employeePictureDao.DestroyPhotoAsync(id);

        public async Task<byte[]> ShowPhotoAsync(int id) => await this.employeePictureDao.ShowPhotoAsync(id);

        public async Task<bool> UpdatePhotoAsync(int id, Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            return await this.employeePictureDao.UpdatePhotoAsync(id, stream);
        }
    }
}
