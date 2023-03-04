CREATE TABLE Employee(
  Emp_Id int IDENTITY(1, 1) PRIMARY KEY NOT NULL,
  Emp_Name varchar(20) NOT NULL,
  Emp_Surname varchar(20) NOT NULL,
  Emp_FatherName varchar(20) NOT NULL,
  Emp_Position varchar(50) NULL,
  Emp_Salary money NOT NULL,
  Emp_JoiningDate datetime NOT NULL
)