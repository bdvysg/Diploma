CREATE OR ALTER PROCEDURE GetEmployeeList
AS
BEGIN
  SELECT Emp_Id AS [���],
         CONCAT(Emp_Surname, ' ', Emp_Name, ' ', Emp_FatherName) AS [��'�],
         Emp_Position AS [������],
         Emp_Salary AS [��������],
         Emp_JoiningDate AS [���� �������� �� ������]
  FROM Employee
END