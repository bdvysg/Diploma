CREATE OR ALTER PROCEDURE GetEmployeeList
AS
BEGIN
  SELECT Emp_Id AS [Код],
         CONCAT(Emp_Surname, ' ', Emp_Name, ' ', Emp_FatherName) AS [Ім'я],
         Emp_Position AS [Посада],
         Emp_Salary AS [Зарплата],
         Emp_JoiningDate AS [Дата прийнятя на роботу]
  FROM Employee
END