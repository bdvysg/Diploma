CREATE OR ALTER PROCEDURE GetReceiptInfo 
  @transno varchar(50)
AS
BEGIN
  SELECT Pr_Title AS [�����],
         Crt_Price AS [ֳ��],
         Crt_Qty AS [ʳ������],
         Crt_Disc_percent AS [������ %],
         Crt_Discount AS [������],
         Crt_Total AS [����],
         (SELECT SUM(Crt_Total) 
          FROM Cart
          WHERE Crt_Transno = @transno) AS [�� ������],
         Crt_Transno AS [����� ����],
         Emp_Name + ' ' + Emp_SurName AS [�����],
         (SELECT Str_Title FROM Store) AS [�������],
         (SELECT [address] FROM Store) AS [������]
  FROM Cart
  INNER JOIN Product
    ON Pr_Id = Crt_Product
  INNER JOIN Employee
    ON Emp_Id = Crt_Cashier
  WHERE Crt_Status = 2 AND
        Crt_Transno = @transno   
END

-- exec GetReceiptInfo '202303131008'