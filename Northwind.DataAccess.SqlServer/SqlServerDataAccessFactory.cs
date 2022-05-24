using System;
using Microsoft.Data.SqlClient;
using Northwind.DataAccess.DAO_s;
using Northwind.DataAccess.SqlServer.SqlDao;

namespace Northwind.DataAccess.SqlServer
{
    /// <summary>
    /// Represents an abstract factory for creating Northwind DAO for SQL Server.
    /// </summary>
    public sealed class SqlServerDataAccessFactory : NorthwindDataAccessFactory
    {
        private readonly SqlConnection sqlConnection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlServerDataAccessFactory"/> class.
        /// </summary>
        /// <param name="sqlConnection">A database connection to SQL Server.</param>
        public SqlServerDataAccessFactory(SqlConnection sqlConnection)
        {
            this.sqlConnection = sqlConnection ?? throw new ArgumentNullException(nameof(sqlConnection));
        }

        /// <inheritdoc/>
        public override IProductCategoryDAO GetProductCategoryDataAccessObject()
        {
            return new ProductCategorySqlServerDao(this.sqlConnection);
        }

        /// <inheritdoc/>
        public override IProductDAO GetProductDataAccessObject()
        {
            return new ProductSqlServerDao(this.sqlConnection);
        }

        /// <inheritdoc />
        public override IEmployeeDAO GetEmployeeDataAccessObject()
        {
            return new EmployeeSqlServerDao(this.sqlConnection);
        }

        /// <inheritdoc />
        public override IEmployeePictureDAO GetEmployeePictureDataAccessObject()
        {
            return new EmployeePictureSqlServerDao(this.sqlConnection);
        }
    }
}
