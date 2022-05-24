namespace Northwind.Serivces.EntityFrameworkCore.Customers
{
    using System;
    using System.Threading.Tasks;
    using Northwind.Services;
    using Northwind.Services.Customers;
    using Northwind.Services.Entities;

#pragma warning disable CA2007

    public class CustomerService : ICustomerService
    {
        private readonly NorthwindContext context;

        public CustomerService(NorthwindContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Customer> GetCustomerAsync(string customerId) => await this.context.Customers.FindAsync(customerId);
    }
}
