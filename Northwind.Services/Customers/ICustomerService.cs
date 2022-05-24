namespace Northwind.Services.Customers
{
    using System.Threading.Tasks;
    using Northwind.Services.Entities;

    public interface ICustomerService
    {
        public Task<Customer> GetCustomerAsync(string customerId);
    }
}
