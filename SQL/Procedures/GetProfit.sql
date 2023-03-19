CREATE OR ALTER PROCEDURE GetProfit
  @DateStart datetime,
  @DateEnd datetime
AS
BEGIN
  SELECT 
    CASE MONTH(Crt_Date)  
      WHEN 1 THEN 'ѳ����'
      WHEN 2 THEN '�����'
      WHEN 3 THEN '��������'
      WHEN 4 THEN '������'
      WHEN 5 THEN '�������'
      WHEN 6 THEN '�������'
      WHEN 7 THEN '������'
      WHEN 8 THEN '�������'
      WHEN 9 THEN '��������'
      WHEN 10 THEN '�������'
      WHEN 11 THEN '��������'
      WHEN 12 THEN '�������'
    END + ' ' + CAST(YEAR(Crt_Date) AS varchar) + '�.' AS [̳����],
    SUM(Crt_Total) - SUM(Pr_PriceOpt) AS [����]
    --SUM(Crt_Total) AS [����]
  FROM Cart
  INNER JOIN Product 
    ON Pr_Id = Crt_Product
  WHERE Crt_Date BETWEEN @DateStart AND @DateEnd
  GROUP BY MONTH(Crt_Date), YEAR(Crt_Date)
END

-- exec GetProfit '20000101', '20231212'