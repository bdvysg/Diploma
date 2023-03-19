CREATE OR ALTER PROCEDURE GetReceiptList
AS
BEGIN
  SELECT 
    Crt_Transno AS [�����],
    Emp_Name + ' ' + Emp_Surname AS [�����],
    Crt_Date AS [����],
    SUM(Crt_Total) AS [����]
  FROM Cart
  INNER JOIN Employee
    ON Emp_Id = Crt_Cashier
  WHERE Crt_Status = 2
  GROUP BY Crt_Transno, Emp_Name, Emp_Surname, Crt_Date
  ORDER BY Crt_Date
END