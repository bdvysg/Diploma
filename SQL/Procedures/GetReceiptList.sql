CREATE OR ALTER PROCEDURE GetReceiptList
AS
BEGIN
  SELECT 
    Crt_Transno AS [Номер],
    Emp_Name + ' ' + Emp_Surname AS [Касир],
    Crt_Date AS [Дата],
    SUM(Crt_Total) AS [Сума]
  FROM Cart
  INNER JOIN Employee
    ON Emp_Id = Crt_Cashier
  WHERE Crt_Status = 2
  GROUP BY Crt_Transno, Emp_Name, Emp_Surname, Crt_Date
  ORDER BY Crt_Date
END