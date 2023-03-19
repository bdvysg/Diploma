CREATE OR ALTER PROCEDURE GetProfit
  @DateStart datetime,
  @DateEnd datetime
AS
BEGIN
  SELECT 
    CASE MONTH(Crt_Date)  
      WHEN 1 THEN 'Січень'
      WHEN 2 THEN 'Лютий'
      WHEN 3 THEN 'Березень'
      WHEN 4 THEN 'Квітень'
      WHEN 5 THEN 'Травень'
      WHEN 6 THEN 'Червень'
      WHEN 7 THEN 'Липень'
      WHEN 8 THEN 'Серпень'
      WHEN 9 THEN 'Вересень'
      WHEN 10 THEN 'Жовтень'
      WHEN 11 THEN 'Листопад'
      WHEN 12 THEN 'Грудень'
    END + ' ' + CAST(YEAR(Crt_Date) AS varchar) + 'р.' AS [Місяць],
    SUM(Crt_Total) - SUM(Pr_PriceOpt) AS [Сума]
    --SUM(Crt_Total) AS [Сума]
  FROM Cart
  INNER JOIN Product 
    ON Pr_Id = Crt_Product
  WHERE Crt_Date BETWEEN @DateStart AND @DateEnd
  GROUP BY MONTH(Crt_Date), YEAR(Crt_Date)
END

-- exec GetProfit '20000101', '20231212'