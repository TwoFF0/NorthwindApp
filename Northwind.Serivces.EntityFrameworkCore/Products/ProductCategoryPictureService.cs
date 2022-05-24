namespace Northwind.Serivces.EntityFrameworkCore.Products
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Northwind.Services.Products;

    /// <inheritdoc/>
    public class ProductCategoryPictureService : IProductCategoryPictureService
    {
#pragma warning disable CA2007
#pragma warning disable CA2208

        private readonly NorthwindContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductCategoryPictureService"/> class.
        /// </summary>
        /// <param name="context">Context to work with.</param>
        public ProductCategoryPictureService(NorthwindContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public async Task<byte[]> ShowPictureAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(id)} cannot be less or equal zero");
            }

            var category = await this.context.ProductCategories.FindAsync(id);

            return category.Picture;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdatePictureAsync(int id, Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(id)} cannot be less or equal zero");
            }

            var category = await this.context.ProductCategories.FindAsync(id);

            if (category != null)
            {
                await using var memoryStream = (MemoryStream)stream;
                byte[] picWrapped = new byte[memoryStream.Length];
                Array.Copy(memoryStream.ToArray(), 0, picWrapped, 0, memoryStream.Length);

                category.Picture = picWrapped;
                await this.context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public async Task<bool> DestroyPictureAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(id)} cannot be less or equal zero");
            }

            var category = await this.context.ProductCategories.FindAsync(id);

            if (category != null)
            {
                category.Picture = null;
                await this.context.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
