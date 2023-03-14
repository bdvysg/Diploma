CREATE OR ALTER PROCEDURE GetReceiptInfo 
  @transno varchar(50)
AS
BEGIN
  SELECT Pr_Title AS [Назва],
         Crt_Price AS [Ціна],
         Crt_Qty AS [Кількість],
         Crt_Disc_percent AS [Знижка %],
         Crt_Discount AS [Знижка],
         Crt_Total AS [Сума],
         (SELECT SUM(Crt_Total) 
          FROM Cart
          WHERE Crt_Transno = @transno) AS [До сплати],
         Crt_Transno AS [Номер чеку],
         Emp_Name + ' ' + Emp_SurName AS [Касир],
         (SELECT Str_Title FROM Store) AS [Магазин],
         (SELECT [address] FROM Store) AS [Адреса]
  FROM Cart
  INNER JOIN Product
    ON Pr_Id = Crt_Product
  INNER JOIN Employee
    ON Emp_Id = Crt_Cashier
  WHERE Crt_Status = 2 AND
        Crt_Transno = @transno   
END

-- exec GetReceiptInfo '202303131008'