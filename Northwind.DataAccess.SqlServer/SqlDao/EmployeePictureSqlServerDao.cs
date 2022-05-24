using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Northwind.DataAccess.DAO_s;
using Northwind.DataAccess.Exceptions;

namespace Northwind.DataAccess.SqlServer.SqlDao
{
    public class EmployeePictureSqlServerDao : IEmployeePictureDAO
    {
        private readonly SqlConnection connection;

        public EmployeePictureSqlServerDao(SqlConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public async Task<bool> DestroyPhotoAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException($"{id} is out of range");
            }

            await using var command = new SqlCommand("DeletePhoto", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            const string employeeId = "@id";
            command.Parameters.Add(employeeId, SqlDbType.Int);
            command.Parameters[employeeId].Value = id;

            await this.connection.OpenAsync();

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<byte[]> ShowPhotoAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException($"{id} is out of range");
            }

            await using var command = new SqlCommand("ShowPhoto", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            const string employeeId = "@id";
            command.Parameters.Add(employeeId, SqlDbType.Int);
            command.Parameters[employeeId].Value = id;

            await this.connection.OpenAsync();

            var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                throw new EmployeeNotFoundException(id);
            }

            var photo = (byte[])reader["Photo"];

            return photo;
        }

        public async Task<bool> UpdatePhotoAsync(int id, Stream stream)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException($"{id} is out of range");
            }

            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            await using var ms = (MemoryStream)stream;
            byte[] picWrapped = new byte[ms.Length];
            Array.Copy(ms.ToArray(), 0, picWrapped, 0, ms.Length);

            await using var command = new SqlCommand("UpdatePhoto", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            const string employeeId = "@id";
            command.Parameters.Add(employeeId, SqlDbType.Int);
            command.Parameters[employeeId].Value = id;

            const string imageParameter = "@image";
            command.Parameters.Add(imageParameter, SqlDbType.Image);
            command.Parameters[imageParameter].IsNullable = true;
            command.Parameters[imageParameter].Value = picWrapped;

            await this.connection.OpenAsync();

            return await command.ExecuteNonQueryAsync() > 0;
        }
    }
}
