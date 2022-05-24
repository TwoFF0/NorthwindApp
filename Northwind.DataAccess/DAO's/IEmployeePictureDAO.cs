using System.IO;
using System.Threading.Tasks;

namespace Northwind.DataAccess.DAO_s
{
    public interface IEmployeePictureDAO
    {
        /// <summary>
        /// Try to show a picture.
        /// </summary>
        /// <param name="id">An identifier.</param>
        /// <returns>True if a product category is exist; otherwise false.</returns>
        Task<byte[]> ShowPhotoAsync(int id);

        /// <summary>
        /// Update a picture.
        /// </summary>
        /// <param name="id">An identifier.</param>
        /// <param name="stream">A <see cref="Stream"/>.</param>
        /// <returns>True if a dataPicture is exist; otherwise false.</returns>
        Task<bool> UpdatePhotoAsync(int id, Stream stream);

        /// <summary>
        /// Destroy a picture.
        /// </summary>
        /// <param name="id">An identifier.</param>
        /// <returns>True if a dataPicture is exist; otherwise false.</returns>
        Task<bool> DestroyPhotoAsync(int id);
    }
}
