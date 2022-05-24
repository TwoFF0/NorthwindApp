#pragma warning disable SA1600

using System.IO;
using System.Threading.Tasks;

namespace Northwind.Services.Products
{
    public interface IProductCategoryPictureService
    {
        /// <summary>
        /// Try to show a picture.
        /// </summary>
        /// <param name="id">An identifier.</param>
        /// <returns>True if a product category is exist; otherwise false.</returns>
        Task<byte[]> ShowPictureAsync(int id);

        /// <summary>
        /// Update a picture.
        /// </summary>
        /// <param name="id">An identifier.</param>
        /// <param name="stream">A <see cref="Stream"/>.</param>
        /// <returns>True if a dataPicture is exist; otherwise false.</returns>
        Task<bool> UpdatePictureAsync(int id, Stream stream);

        /// <summary>
        /// Destroy a picture.
        /// </summary>
        /// <param name="id">An identifier.</param>
        /// <returns>True if a dataPicture is exist; otherwise false.</returns>
        Task<bool> DestroyPictureAsync(int id);
    }
}
