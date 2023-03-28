CREATE OR ALTER PROCEDURE GetProductsListWithCrit  -- Процедура выводит список продуктов для формы Product
  @SearchText varchar(200),
  @StartIndex int,
  @RowsCount int
  AS
BEGIN
-- Create a temporary table to store the results
  DROP TABLE IF EXISTS #Result
  CREATE TABLE #Results (
    Pr_Id int,
    Pr_Title varchar(100),
    Pr_Qty varchar(20),
    Br_Title varchar(50),
    Pr_BarCode varchar(50),
    Catg_Title varchar(50),
    Pr_PriceOpt decimal(18,2),
    Pr_Price decimal(18,2),
    Pr_Reorder int,
    OnStk_Quantity int,
    IsCritical bit
  )

  -- Insert the results of the first query into the temporary table
  INSERT INTO #Results (
    Pr_Id,
    Pr_Title,
    Pr_Qty,
    Br_Title,
    Pr_BarCode,
    Catg_Title,
    Pr_PriceOpt,
    Pr_Price,
    Pr_Reorder,
    OnStk_Quantity,
    IsCritical
  )
  SELECT
    P.Pr_Id,
    p.Pr_Title,
    p.Pr_Qty,
    b.Br_Title,
    p.Pr_BarCode,
    c.Catg_Title,
    p.Pr_PriceOpt,
    p.Pr_Price,
    p.Pr_Reorder,
    OnStk_Quantity,
    CASE WHEN OnStk_Quantity < Pr_Reorder THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS IsCritical
  FROM
    Product p
    INNER JOIN Brand b ON b.Br_Id = p.Pr_Brand
    INNER JOIN Category c ON c.Catg_Id = p.Pr_Category
    INNER JOIN OnStock ON OnStk_Product = p.Pr_Id
  WHERE
    OnStk_Quantity < Pr_Reorder

  -- Insert the results of the second query into the temporary table
  INSERT INTO #Results (
    Pr_Id,
    Pr_Title,
    Pr_Qty,
    Br_Title,
    Pr_BarCode,
    Catg_Title,
    Pr_PriceOpt,
    Pr_Price,
    Pr_Reorder,
    OnStk_Quantity,
    IsCritical
  )
 SELECT P.Pr_Id,
         p.Pr_Title, 
         p.Pr_Qty, 
         b.Br_Title, 
         p.Pr_BarCode, 
         c.Catg_Title, 
         p.Pr_PriceOpt,
         p.Pr_Price, 
         p.Pr_Reorder,
         OnStk_Quantity,
         CASE WHEN OnStk_Quantity < Pr_Reorder THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS IsCritical
  FROM Product p
  INNER JOIN Brand AS b 
    ON b.Br_Id = p.Pr_Brand 
  INNER JOIN Category AS c 
    ON c.Catg_Id = p.Pr_Category 
  INNER JOIN OnStock 
    ON OnStk_Product = p.Pr_Id
  WHERE  CONCAT(b.Br_Title, c.Catg_Title, p.Pr_Title) LIKE '%' + @SearchText + '%'
  ORDER BY p.Pr_Id
  OFFSET @StartIndex ROWS
  FETCH NEXT @RowsCount ROWS ONLY;

  SELECT * FROM #Results
END



-- exec GetProductsList '', 0, 10