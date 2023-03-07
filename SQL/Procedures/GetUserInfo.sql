CREATE OR ALTER PROCEDURE GetUserInfo
  @username varchar(100),
  @password varchar(100)
AS
BEGIN
  SELECT Usr_Username,
         Usr_Password,
         Usr_IsActivate,
         CONCAT(Emp_Name, ' ', Emp_Surname) AS Usr_Name,
         Emp_Position,
         Ur_Title
  FROM [User]  
  INNER JOIN Employee
    ON Emp_Id = Usr_Employee
  INNER JOIN UserRole
    ON Ur_Id = Usr_Role
  WHERE 
    Usr_Username = @username AND
    Usr_Password = @password   
END

--exec GetUserInfo '1111', '1111'