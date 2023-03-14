CREATE OR ALTER PROCEDURE GetTopSellingProducts
  @DateStart datetime,
  @DateEnd datetime,
  @Count int
AS
BEGIN
  SELECT TOP (@Count) 
         Pr_Id AS [Код товару],
         Pr_Title [Назва],
         SUM(Crt_Qty) as [Кількість]
  FROM Product
  INNER JOIN Cart 
    ON Crt_Product = Pr_Id
  WHERE 
    Crt_Status = 2 AND
    Crt_Date BETWEEN @DateStart AND @DateEnd    
  GROUP BY Crt_Product, Pr_Title, Pr_Id
  ORDER BY [Кількість] DESC
END

--exec GetTopSellingProducts '20000101', '20231212', 100 