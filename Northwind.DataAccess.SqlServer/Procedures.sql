CREATE PROCEDURE "DeleteEmployee"
  @employeeId int
AS 
  DELETE FROM Employees
  WHERE Employees.EmployeeID = @employeeId

GO
 CREATE PROCEDURE "InsertEmployee"
  @firstName nvarchar(10),
  @lastName nvarchar(20),
  @title nvarchar(30),
  @titleOfCourtesy nvarchar(25),
  @address nvarchar(60),
  @city nvarchar(15),
  @region nvarchar(15),
  @postalCode nvarchar(10),
  @country nvarchar(15),
  @homePhone nvarchar(24),
  @extension nvarchar(4),
  @photoPath nvarchar(255),
  @birthDate datetime,
  @hireDate datetime,
  @image image,
  @notes ntext,
  @reportsTo int

AS 
  INSERT INTO Employees (FirstName, LastName, Title, TitleOfCourtesy, Address, City, Region, PostalCode, Country, HomePhone, Extension, PhotoPath, BirthDate, HireDate, Photo, Notes, ReportsTo)
  OUTPUT Inserted.EmployeeId
  VALUES(@firstName, @lastName, @title, @titleOfCourtesy, @address, @city, @region, @postalCode, @country, @homePhone, @extension, @photoPath, @birthDate, @hireDate, @image, @notes, @reportsTo)

GO
 CREATE PROCEDURE "UpdateEmployee"
  @employeeId int,
  @firstName nvarchar(10),
  @lastName nvarchar(20),
  @title nvarchar(30),
  @titleOfCourtesy nvarchar(25),
  @address nvarchar(60),
  @city nvarchar(15),
  @region nvarchar(15),
  @postalCode nvarchar(10),
  @country nvarchar(15),
  @homePhone nvarchar(24),
  @extension nvarchar(4),
  @photoPath nvarchar(255),
  @birthDate datetime,
  @hireDate datetime,
  @image image,
  @notes ntext,
  @reportsTo int

AS 
  UPDATE Employees
  SET FirstName = @firstName, LastName = @lastName, Title = @title, TitleOfCourtesy = @titleOfCourtesy, Address = @address, City = @city, Region = @region, PostalCode = @postalCode, Country = @country, HomePhone = @homePhone, Extension = @extension, PhotoPath = @photoPath, BirthDate = @birthDate, HireDate = @hireDate, Photo = @image, Notes = @notes, ReportsTo = @reportsTo
  WHERE Employees.EmployeeID = @employeeId

GO
  CREATE PROCEDURE "FindEmployee"
  @employeeId int
AS 
  SELECT * FROM Employees
  WHERE Employees.EmployeeID = @employeeId

GO
  CREATE PROCEDURE "SelectEmployeesOffset"
  @offset int,
  @limit int
AS 
  SELECT COUNT(@limit) (SELECT SKIP(@offset) FROM Employees)

GO
  CREATE PROCEDURE "SelectEmployees"
AS 
  SELECT * FROM Employees

GO
  CREATE PROCEDURE "DeletePhoto"
  @id int
AS 
  UPDATE Employees
  SET Photo = NULL
  WHERE Employees.EmployeeID = @id

GO
  CREATE PROCEDURE "ShowPhoto"
  @id int
AS 
  SELECT Employees.Photo FROM Employees
  WHERE Employees.EmployeeID = @id

GO
  CREATE PROCEDURE "UpdatePhoto"
  @id int,
  @image image
AS 
  UPDATE Employees
  SET Photo = @image
  WHERE Employees.EmployeeID = @id

