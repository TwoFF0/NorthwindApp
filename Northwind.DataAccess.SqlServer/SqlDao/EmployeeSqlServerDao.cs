using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Northwind.DataAccess.DAO_s;
using Northwind.DataAccess.Exceptions;
using Northwind.DataAccess.TransferObjects;

#pragma warning disable CS8073

namespace Northwind.DataAccess.SqlServer.SqlDao
{
    /// <summary>
    /// Represents a SQL Server-tailored DAO for Northwind products.
    /// </summary>
    public sealed class EmployeeSqlServerDao : IEmployeeDAO
    {
        private readonly SqlConnection connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeSqlServerDao"/> class.
        /// </summary>
        /// <param name="connection">A <see cref="SqlConnection"/>.</param>
        public EmployeeSqlServerDao(SqlConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public async Task<int> InsertEmployeeAsync(EmployeeTransferObject employee)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            await using var command = new SqlCommand("InsertEmployee", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            AddSqlParameters(employee, command);

            await this.connection.OpenAsync();
            var id = await command.ExecuteNonQueryAsync();

            return id;
        }

        public async Task<bool> DeleteEmployeeAsync(int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentOutOfRangeException($"{employeeId} is out of range");
            }

            await using var command = new SqlCommand("DeleteEmployee", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            const string id = "@employeeId";
            command.Parameters.Add(id, SqlDbType.Int);
            command.Parameters[id].Value = employeeId;

            await this.connection.OpenAsync();

            return await command.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> UpdateEmployeeAsync(EmployeeTransferObject employee)
        {
            if (employee is null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            await using var command = new SqlCommand("UpdateEmployee", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            AddSqlParameters(employee, command);

            const string employeeId = "@employeeId";
            command.Parameters.Add(employeeId, SqlDbType.Int);
            command.Parameters[employeeId].Value = employee.Id;

            await this.connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync() > 0;

            return result;
        }

        public async Task<EmployeeTransferObject> FindEmployeeAsync(int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new ArgumentOutOfRangeException($"{employeeId} is out of range");
            }

            await using var command = new SqlCommand("FindEmployee", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            const string id = "@employeeId";
            command.Parameters.Add(id, SqlDbType.Int);
            command.Parameters[id].Value = employeeId;

            await this.connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            if (!reader.Read())
            {
                throw new EmployeeNotFoundException(employeeId);
            }

            var createdEmployee = CreateEmployee(reader);
            await this.connection.CloseAsync();

            return createdEmployee;
        }

        public async IAsyncEnumerable<EmployeeTransferObject> SelectEmployeesAsync(int offset, int limit)
        {
            if (offset < 0)
            {
                throw new ArgumentException("Must be greater than zero or equals zero.", nameof(offset));
            }

            if (limit < 1)
            {
                throw new ArgumentException("Must be greater than zero.", nameof(limit));
            }

            await using var command = new SqlCommand("SelectEmployeesOffset", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            const string skip = "@offset";
            command.Parameters.Add(skip, SqlDbType.Int);
            command.Parameters[skip].Value = offset;

            const string take = "@limit";
            command.Parameters.Add(take, SqlDbType.Int);
            command.Parameters[take].Value = limit;

            await this.connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                yield return CreateEmployee(reader);
            }
        }

        public async IAsyncEnumerable<EmployeeTransferObject> SelectAllEmployeeAsync()
        {
            await using var command = new SqlCommand("SelectEmployees", this.connection)
            {
                CommandType = CommandType.StoredProcedure,
            };

            await this.connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                yield return CreateEmployee(reader);
            }
        }

        private static EmployeeTransferObject CreateEmployee(SqlDataReader reader)
        {
            var id = (int)reader["EmployeeID"];
            var firstName = (string)reader["FirstName"];
            var lastName = (string)reader["LastName"];

            const string titleColumnName = "Title";
            string title = null;

            if (reader[titleColumnName] != DBNull.Value)
            {
                title = (string)reader[titleColumnName];
            }

            const string titleOfCourtesyColumnName = "TitleOfCourtesy";
            string titleOfCourtesy = null;

            if (reader[titleOfCourtesyColumnName] != DBNull.Value)
            {
                titleOfCourtesy = (string)reader[titleOfCourtesyColumnName];
            }

            const string addressColumnName = "Address";
            string address = null;

            if (reader[addressColumnName] != DBNull.Value)
            {
                address = (string)reader[addressColumnName];
            }

            const string cityColumnName = "City";
            string city = null;

            if (reader[cityColumnName] != DBNull.Value)
            {
                city = (string)reader[cityColumnName];
            }

            const string regionColumnName = "Region";
            string region = null;

            if (reader[regionColumnName] != DBNull.Value)
            {
                region = (string)reader[regionColumnName];
            }

            const string postalCodeColumnName = "PostalCode";
            string postalCode = null;

            if (reader[postalCodeColumnName] != DBNull.Value)
            {
                postalCode = (string)reader[postalCodeColumnName];
            }

            const string countryColumnName = "Country";
            string country = null;

            if (reader[countryColumnName] != DBNull.Value)
            {
                country = (string)reader[countryColumnName];
            }

            const string homePhoneColumnName = "HomePhone";
            string homePhone = null;

            if (reader[homePhoneColumnName] != DBNull.Value)
            {
                homePhone = (string)reader[homePhoneColumnName];
            }

            const string extensionColumnName = "Extension";
            string extension = null;

            if (reader[extensionColumnName] != DBNull.Value)
            {
                extension = (string)reader[extensionColumnName];
            }

            const string photoPathColumnName = "PhotoPath";
            string photoPath = null;

            if (reader[photoPathColumnName] != DBNull.Value)
            {
                photoPath = (string)reader[photoPathColumnName];
            }

            const string birthDateColumnName = "BirthDate";
            DateTime? birthDate = null;

            if (reader[birthDateColumnName] != DBNull.Value)
            {
                birthDate = (DateTime)reader[birthDateColumnName];
            }

            const string hireDateColumnName = "HireDate";
            DateTime? hireDate = null;

            if (reader[hireDateColumnName] != DBNull.Value)
            {
                hireDate = (DateTime)reader[hireDateColumnName];
            }

            const string photoColumnName = "Photo";
            byte[] photo = null;

            if (reader[photoColumnName] != DBNull.Value)
            {
                photo = (byte[])reader[photoColumnName];
            }

            const string notesColumnName = "Notes";
            string notes = null;

            if (reader[notesColumnName] != DBNull.Value)
            {
                notes = (string)reader[notesColumnName];
            }

            const string reportsToColumnName = "ReportsTo";
            int? reportsTo = null;

            if (reader[reportsToColumnName] != DBNull.Value)
            {
                reportsTo = (int)reader[reportsToColumnName];
            }

            return new EmployeeTransferObject()
            {
                Id = id,
                Photo = photo,
                BirthDate = birthDate,
                Address = address,
                City = city,
                FirstName = firstName,
                Country = country,
                Extension = extension,
                HireDate = hireDate,
                HomePhone = homePhone,
                LastName = lastName,
                TitleOfCourtesy = titleOfCourtesy,
                Region = region,
                PostalCode = postalCode,
                Notes = notes,
                PhotoPath = photoPath,
                ReportsTo = reportsTo,
                Title = title,
            };
        }

        private static void AddSqlParameters(EmployeeTransferObject employee, SqlCommand command)
        {
            const string firstNameParameter = "@firstName";
            command.Parameters.Add(firstNameParameter, SqlDbType.NVarChar, 10);
            command.Parameters[firstNameParameter].Value = employee.FirstName;

            const string lastNameParameter = "@lastName";
            command.Parameters.Add(lastNameParameter, SqlDbType.NVarChar, 20);
            command.Parameters[lastNameParameter].Value = employee.LastName;

            const string titleParameter = "@title";
            command.Parameters.Add(titleParameter, SqlDbType.NVarChar, 30);
            command.Parameters[titleParameter].IsNullable = true;

            if (employee.Title != null)
            {
                command.Parameters[titleParameter].Value = employee.Title;
            }
            else
            {
                command.Parameters[titleParameter].Value = DBNull.Value;
            }

            const string titleOfCourtesyParameter = "@titleOfCourtesy";
            command.Parameters.Add(titleOfCourtesyParameter, SqlDbType.NVarChar, 25);
            command.Parameters[titleOfCourtesyParameter].IsNullable = true;

            if (employee.TitleOfCourtesy != null)
            {
                command.Parameters[titleOfCourtesyParameter].Value = employee.TitleOfCourtesy;
            }
            else
            {
                command.Parameters[titleOfCourtesyParameter].Value = DBNull.Value;
            }

            const string addressParameter = "@address";
            command.Parameters.Add(addressParameter, SqlDbType.NVarChar, 60);
            command.Parameters[addressParameter].IsNullable = true;

            if (employee.Address != null)
            {
                command.Parameters[addressParameter].Value = employee.Address;
            }
            else
            {
                command.Parameters[addressParameter].Value = DBNull.Value;
            }

            const string cityParameter = "@city";
            command.Parameters.Add(cityParameter, SqlDbType.NVarChar, 15);
            command.Parameters[cityParameter].IsNullable = true;

            if (employee.City != null)
            {
                command.Parameters[cityParameter].Value = employee.City;
            }
            else
            {
                command.Parameters[cityParameter].Value = DBNull.Value;
            }

            const string regionParameter = "@region";
            command.Parameters.Add(regionParameter, SqlDbType.NVarChar, 15);
            command.Parameters[regionParameter].IsNullable = true;

            if (employee.Region != null)
            {
                command.Parameters[regionParameter].Value = employee.Region;
            }
            else
            {
                command.Parameters[regionParameter].Value = DBNull.Value;
            }

            const string postalCodeParameter = "@postalCode";
            command.Parameters.Add(postalCodeParameter, SqlDbType.NVarChar, 10);
            command.Parameters[postalCodeParameter].IsNullable = true;

            if (employee.PostalCode != null)
            {
                command.Parameters[postalCodeParameter].Value = employee.PostalCode;
            }
            else
            {
                command.Parameters[postalCodeParameter].Value = DBNull.Value;
            }

            const string countryParameter = "@country";
            command.Parameters.Add(countryParameter, SqlDbType.NVarChar, 15);
            command.Parameters[countryParameter].IsNullable = true;

            if (employee.Country != null)
            {
                command.Parameters[countryParameter].Value = employee.Country;
            }
            else
            {
                command.Parameters[countryParameter].Value = DBNull.Value;
            }

            const string homePhoneParameter = "@homePhone";
            command.Parameters.Add(homePhoneParameter, SqlDbType.NVarChar, 24);
            command.Parameters[homePhoneParameter].IsNullable = true;

            if (employee.HomePhone != null)
            {
                command.Parameters[homePhoneParameter].Value = employee.HomePhone;
            }
            else
            {
                command.Parameters[homePhoneParameter].Value = DBNull.Value;
            }

            const string extensionParameter = "@extension";
            command.Parameters.Add(extensionParameter, SqlDbType.NVarChar, 4);
            command.Parameters[extensionParameter].IsNullable = true;

            if (employee.Extension != null)
            {
                command.Parameters[extensionParameter].Value = employee.Extension;
            }
            else
            {
                command.Parameters[extensionParameter].Value = DBNull.Value;
            }

            const string photoPathParameter = "@photoPath";
            command.Parameters.Add(photoPathParameter, SqlDbType.NVarChar, 255);
            command.Parameters[photoPathParameter].IsNullable = true;

            if (employee.PhotoPath != null)
            {
                command.Parameters[photoPathParameter].Value = employee.PhotoPath;
            }
            else
            {
                command.Parameters[photoPathParameter].Value = DBNull.Value;
            }

            const string birthDateParameter = "@birthDate";
            command.Parameters.Add(birthDateParameter, SqlDbType.DateTime);
            command.Parameters[birthDateParameter].IsNullable = true;

            if (employee.BirthDate != null)
            {
                command.Parameters[birthDateParameter].Value = employee.BirthDate;
            }
            else
            {
                command.Parameters[birthDateParameter].Value = DBNull.Value;
            }

            const string hireDateParameter = "@hireDate";
            command.Parameters.Add(hireDateParameter, SqlDbType.DateTime);
            command.Parameters[hireDateParameter].IsNullable = true;

            if (employee.HireDate != null)
            {
                command.Parameters[hireDateParameter].Value = employee.HireDate;
            }
            else
            {
                command.Parameters[hireDateParameter].Value = DBNull.Value;
            }

            const string imageParameter = "@image";
            command.Parameters.Add(imageParameter, SqlDbType.Image);
            command.Parameters[imageParameter].IsNullable = true;

            if (employee.Photo != null)
            {
                command.Parameters[imageParameter].Value = employee.Photo;
            }
            else
            {
                command.Parameters[imageParameter].Value = DBNull.Value;
            }

            const string notesParameter = "@notes";
            command.Parameters.Add(notesParameter, SqlDbType.NText);
            command.Parameters[notesParameter].IsNullable = true;

            if (employee.Notes != null)
            {
                command.Parameters[notesParameter].Value = employee.Notes;
            }
            else
            {
                command.Parameters[notesParameter].Value = DBNull.Value;
            }


            const string reportsToParameter = "@reportsTo";
            command.Parameters.Add(reportsToParameter, SqlDbType.Int);
            command.Parameters[reportsToParameter].IsNullable = true;

            if (employee.ReportsTo != null)
            {
                command.Parameters[reportsToParameter].Value = employee.ReportsTo;
            }
            else
            {
                command.Parameters[reportsToParameter].Value = DBNull.Value;
            }
        }
    }
}
