using Northwind.DataAccess.DAO_s;

namespace Northwind.DataAccess
{
    /// <summary>
    /// Represents an abstract factory for creating Northwind DAO.
    /// </summary>
    public abstract class NorthwindDataAccessFactory
    {
        /// <summary>
        /// Gets a DAO for Northwind products.
        /// </summary>
        /// <returns>A <see cref="IProductDAO"/>.</returns>
        public abstract IProductDAO GetProductDataAccessObject();

        /// <summary>
        /// Gets a DAO for Northwind product categories.
        /// </summary>
        /// <returns>A <see cref="IProductCategoryDAO"/>.</returns>
        public abstract IProductCategoryDAO GetProductCategoryDataAccessObject();

        /// <summary>
        /// Gets a DAO for Northwind employees.
        /// </summary>
        /// <returns>A <see cref="IEmployeeDAO"/>.</returns>
        public abstract IEmployeeDAO GetEmployeeDataAccessObject();

        /// <summary>
        /// Gets a DAO for Northwind employee pictures.
        /// </summary>
        /// <returns>A <see cref="IEmployeePictureDAO"/>.</returns>
        public abstract IEmployeePictureDAO GetEmployeePictureDataAccessObject();
    }
}
